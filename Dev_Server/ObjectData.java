/*
Implements the the Client API, designed for custom low level communication as
outlined in Hololens "Communication V1.png".
*/

public class ObjectData {
    private int xCo;
    private int yCo;
    private int zCo;
    private String type;

    ObjectData(int xCo, int yCo, int zCo, int type) {
        this.xCo = xCo;
        this.yCo = yCo;
        this.zCo = zCo;
        this.type = type;
    }

    public String getObjectAsString() {
        String returnString = "";
        returnString += Integer.toString(xCo) + ";";
        returnString += Integer.toString(yCo) + ";";
        returnString += Integer.toString(zCo) + ";";
        returnString += Integer.toString(type);
    }
}
