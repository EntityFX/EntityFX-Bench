package utils

import "fmt"

type WriterType interface {
	Write(format string, a ...interface{}) (n int, err error)

	WriteLine(format string, a ...interface{}) (n int, err error)

	WriteColor(color string, format string, a ...interface{}) (n int, err error)

	WriteNewLine() (n int, err error)

	WriteHeader(format string, a ...interface{}) (n int, err error)

	WriteValue(format string, a ...interface{}) (n int, err error)

	WriteTitle(format string, a ...interface{}) (n int, err error)
}

type Writer struct {
	useConsole bool
	filePath   string
	output     string
}

func NewWriter(filePath string) WriterType {
	return &Writer{true, filePath, ""}
}

func (w *Writer) WriteColor(color string, format string, a ...interface{}) (n int, err error) {
	var formatted = fmt.Sprintf(format, a...)
	if w.useConsole {
		n, err = fmt.Print(color + formatted + "\033[0m")
		return n, err
	}
	w.output += formatted

	return n, err
}

func (w *Writer) WriteNewLine() (n int, err error) {
	return fmt.Println()
}

func (w *Writer) Write(format string, a ...interface{}) (n int, err error) {
	return w.WriteColor("\033[1;30m", format, a...)
}

func (w *Writer) WriteLine(format string, a ...interface{}) (n int, err error) {
	n, err = w.WriteColor("\033[1;30m", format, a...)
	w.WriteNewLine()
	return n, err
}

func (w *Writer) WriteHeader(format string, a ...interface{}) (n int, err error) {
	n, err = w.WriteColor("\033[1;36m", format, a...)
	w.WriteNewLine()
	return n, err
}

func (w *Writer) WriteValue(format string, a ...interface{}) (n int, err error) {
	return w.WriteColor("\033[1;32m", format, a...)
}

func (w *Writer) WriteTitle(format string, a ...interface{}) (n int, err error) {
	return w.WriteColor("\033[1;37m", format, a...)
}
