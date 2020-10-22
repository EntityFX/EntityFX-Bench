package EntityFX.Core;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

public class Writer {

    private final String writer = "";

    public boolean UseConsole = true;

    public String FilePath = null;

    public String Output;

    private FileOutputStream fileStream;

    public Writer(final String filePath) throws FileNotFoundException {
        if (filePath != null) {
            this.FilePath = filePath;
            this.fileStream = new FileOutputStream(filePath, true);
        }
    }

    public void writeLine(final String format, Object... args) throws IOException {
        this.writeColor("\033[1;30m", format, args);
        this.writeLine();
    }

    public void writeLine() throws IOException
    {
        if (this.UseConsole) {
            if (this.FilePath != null) {
                fileStream.write("\n".getBytes());
            }
            System.out.println();
        }
        this.Output += "\n";
    }

    public void writeHeader(String format, Object... args) throws IOException
    {
        this.writeColor("\033[1;36m", format, args);
        this.writeLine();
    }

    public void write(String format, Object... args) throws IOException
    {
        this.writeColor("\033[1;30m", format, args);
    }

    public void writeColor(final String color, String format, Object... args) throws IOException
    {
        var formatted = String.format(format, args);
        if (this.UseConsole)
        {
            System.out.print(color + formatted + "\033[0m");

            if (this.FilePath != null) {
                fileStream.write(formatted.getBytes());
            }
        }
        this.Output += formatted;
    }

    public void writeValue(String format, Object... args) throws IOException
    {
        this.writeColor("\033[1;32m", format, args);
    }

    public void writeTitle(String format, Object... args) throws IOException
    {
        this.writeColor("\033[1;37m", format, args);
    }
    
}