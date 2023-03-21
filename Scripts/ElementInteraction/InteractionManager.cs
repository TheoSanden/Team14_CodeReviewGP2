using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Events.InteractionEventListener))]
public class InteractionManager : MonoBehaviour
{
    [Header("Entity Events")]
    [SerializeField] UnityEvent<Transform> E_Fire_Wind;
    [SerializeField] UnityEvent<Transform> E_Fire_Sludge;
    [SerializeField] UnityEvent<Transform> E_Radiation_Wind;
    [SerializeField] UnityEvent<Transform> E_Radiation_Sludge;
    [Header("Global Events")]
    [SerializeField] UnityEvent<Vector3> G_Fire_Wind;
    [SerializeField] UnityEvent<Vector3> G_Fire_Sludge;
    [SerializeField] UnityEvent<Vector3> G_Radiation_Wind;
    [SerializeField] UnityEvent<Vector3> G_Radiation_Sludge;

    public void PrintPosition(Vector3 position)
    {
        print(position);
    }
    //EWWWWWWWWWW I might change this later when im smarter
    public void HandleInteractionEvents(InteractionArgs args)
    {
        Debug.Log(args._InteractionType);
        switch (args._InteractionType)
        {
            case InteractionArgs.InteractionType.entity:
                switch (args.ElementType_1)
                {
                    case InteractionArgs.ElementType.fire:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.wind: E_Fire_Wind?.Invoke(args.TargetTransform); break;
                            case InteractionArgs.ElementType.sludge: E_Fire_Sludge?.Invoke(args.TargetTransform); break;
                        }
                        break;
                    case InteractionArgs.ElementType.radiation:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.wind: E_Radiation_Wind?.Invoke(args.TargetTransform); break;
                            case InteractionArgs.ElementType.sludge: E_Radiation_Sludge?.Invoke(args.TargetTransform); break;
                        }
                        break;
                    case InteractionArgs.ElementType.wind:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.fire: E_Fire_Wind?.Invoke(args.TargetTransform); break;
                            case InteractionArgs.ElementType.radiation: E_Radiation_Wind?.Invoke(args.TargetTransform); break;
                        }
                        break;
                    case InteractionArgs.ElementType.sludge:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.fire: E_Fire_Sludge?.Invoke(args.TargetTransform); break;
                            case InteractionArgs.ElementType.radiation: E_Radiation_Sludge?.Invoke(args.TargetTransform); break;
                        }
                        break;
                }
                break;
            case InteractionArgs.InteractionType.global:
                switch (args.ElementType_1)
                {
                    case InteractionArgs.ElementType.fire:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.wind: G_Fire_Wind?.Invoke(args.Position); break;
                            case InteractionArgs.ElementType.sludge: G_Fire_Sludge?.Invoke(args.Position); break;
                        }
                        break;
                    case InteractionArgs.ElementType.radiation:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.wind: G_Radiation_Wind?.Invoke(args.Position); break;
                            case InteractionArgs.ElementType.sludge: G_Radiation_Sludge?.Invoke(args.Position); break;
                        }
                        break;
                    case InteractionArgs.ElementType.wind:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.fire: G_Fire_Wind?.Invoke(args.Position); break;
                            case InteractionArgs.ElementType.radiation: G_Radiation_Wind?.Invoke(args.Position); break;
                        }
                        break;
                    case InteractionArgs.ElementType.sludge:
                        switch (args.ElementType_2)
                        {
                            case InteractionArgs.ElementType.fire: G_Fire_Sludge?.Invoke(args.Position); break;
                            case InteractionArgs.ElementType.radiation: G_Radiation_Sludge?.Invoke(args.Position); break;
                        }
                        break;
                }
                break;
        }
    }
}
