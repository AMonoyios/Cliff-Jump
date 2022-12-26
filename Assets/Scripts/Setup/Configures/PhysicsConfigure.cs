using UnityEngine;

// Physics settings
[System.Serializable]
public class PhysicsConfigure
{
    // Enum to switch between in editor
    [SerializeField]
    public enum Planets
    {
        Custom,
        Earth,
        Moon,
        Mars,
        Venus
    }

    public Planets planet;
    [ConditionalHide(nameof(planet), true, true), Range(-20.0f, -0.1f)]
    public float customGravity = -10.0f;
    public float gravityScale = 3.0f;

    // Method to get the gravity depending on what planet is selected in editor
    public float GetGravity
    {
        get
        {
            switch (planet)
            {
                case Planets.Earth:
                {
                    return -9.8f;
                }
                case Planets.Moon:
                {
                    return -1.6f;
                }
                case Planets.Mars:
                {
                    return -3.7f;
                }
                case Planets.Venus:
                {
                    return -8.8f;
                }
            }
            return customGravity;
        }
    }
}
