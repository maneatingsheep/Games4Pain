[System.Serializable]
public class BodyPart {

    public enum BodyPartTypes {None, Foot, Leg, Back, Wrist, Head };

    public BodyPartTypes Type;
    public string Name;
    public BodyPartTypes[] ExcludedType;
}
