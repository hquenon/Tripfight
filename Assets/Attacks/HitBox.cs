using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ColliderState
{

    Closed,

    Open,

    Colliding

}

public interface IHitboxResponder
{
   void collisionedWith(Collider2D collider);

}
public class HitBox : MonoBehaviour
{
    private IHitboxResponder responder = null;
    public LayerMask mask;

    public bool useSphere = false;

    public Vector3 hitboxSize = Vector3.one;

    public float radius = 0.5f;

    public Color inactiveColor;

    public Color collisionOpenColor;

    public Color collidingColor;


    private ColliderState _state;


    // Start is called before the first frame update
    void Start()
    {
        startCheckingCollision();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_state == ColliderState.Closed) { return; }

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, hitboxSize, 0, mask);

        for(int i=0; i<colliders.Length; i++)
        {
            responder.collisionedWith(colliders[i]);
        }


        if (colliders.Length > 0)
        {
            _state = ColliderState.Colliding;
        }
        else
        {
            _state = ColliderState.Open;
        }


    }

    public void startCheckingCollision()
    {
        _state = ColliderState.Open;

    }

    public void stopCheckingCollision()
    {
        _state = ColliderState.Closed;

    }

    public void useResponder(IHitboxResponder responder)
    {
        this.responder = responder;

    }

    private void OnDrawGizmos()
    {
        checkGizmoColor();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawCube(Vector3.zero, new Vector3(hitboxSize.x , hitboxSize.y , hitboxSize.z )); // Because size is halfExtents

    }

    private void checkGizmoColor()
    {
        switch (_state)
        {

            case ColliderState.Closed:

                Gizmos.color = inactiveColor;

                break;

            case ColliderState.Open:

                Gizmos.color = collisionOpenColor;

                break;

            case ColliderState.Colliding:

                Gizmos.color = collidingColor;

                break;

        }

    }
}
