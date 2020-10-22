
package EntityFX.Core.Whetstone;

public class WhetstoneResult {

    private String _output;

    public String getOutput() {
        return _output;
    }

    public String setOutput(String value) {
        _output = value;
        return _output;
    }

    private double _mwips;

    public double getMWIPS() {
        return _mwips;
    }

    public double setMWIPS(double value) {
        _mwips = value;
        return _mwips;
    }

    private double _timeused;

    public double getTimeUsed() {
        return _timeused;
    }

    public double setTimeUsed(double value) {
        _timeused = value;
        return _timeused;
    }

    public static WhetstoneResult _new1(String _arg1, double _arg2, double _arg3) {
        WhetstoneResult res = new WhetstoneResult();
        res.setOutput(_arg1);
        res.setMWIPS(_arg2);
        res.setTimeUsed(_arg3);
        return res;
    }

    public WhetstoneResult() {
    }
}
