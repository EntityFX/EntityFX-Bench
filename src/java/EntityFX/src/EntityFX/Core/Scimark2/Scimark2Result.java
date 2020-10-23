package EntityFX.Core.Scimark2;


public class Scimark2Result {

    private double _compositescore;

    public double getCompositeScore() {
        return _compositescore;
    }

    public double setCompositeScore(double value) {
        _compositescore = value;
        return _compositescore;
    }


    private double _sor;

    public double getSOR() {
        return _sor;
    }

    public double setSOR(double value) {
        _sor = value;
        return _sor;
    }


    private double _fft;

    public double getFFT() {
        return _fft;
    }

    public double setFFT(double value) {
        _fft = value;
        return _fft;
    }


    private double _montecarlo;

    public double getMonteCarlo() {
        return _montecarlo;
    }

    public double setMonteCarlo(double value) {
        _montecarlo = value;
        return _montecarlo;
    }


    private double _sparsemathmult;

    public double getSparseMathmult() {
        return _sparsemathmult;
    }

    public double setSparseMathmult(double value) {
        _sparsemathmult = value;
        return _sparsemathmult;
    }


    private double _lu;

    public double getLU() {
        return _lu;
    }

    public double setLU(double value) {
        _lu = value;
        return _lu;
    }


    private String _output;

    public String getOutput() {
        return _output;
    }

    public String setOutput(String value) {
        _output = value;
        return _output;
    }


    public static Scimark2Result _new1(double _arg1, double _arg2, double _arg3, double _arg4, double _arg5, double _arg6, String _arg7) {
        Scimark2Result res = new Scimark2Result();
        res.setCompositeScore(_arg1);
        res.setFFT(_arg2);
        res.setSOR(_arg3);
        res.setMonteCarlo(_arg4);
        res.setSparseMathmult(_arg5);
        res.setLU(_arg6);
        res.setOutput(_arg7);
        return res;
    }
    public Scimark2Result() {
    }
}
