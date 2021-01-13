package utils

import (
	"fmt"
	"os"
	"runtime"
	"sync"
)

type WriterType interface {
	Write(format string, a ...interface{}) (n int, err error)

	WriteLine(format string, a ...interface{}) (n int, err error)

	WriteColor(color string, format string, a ...interface{}) (n int, err error)

	WriteNewLine() (n int, err error)

	WriteHeader(format string, a ...interface{}) (n int, err error)

	WriteValue(format string, a ...interface{}) (n int, err error)

	WriteTitle(format string, a ...interface{}) (n int, err error)

	UseConsole(value bool)

	GetOutput() string
}

type Writer struct {
	useConsole bool
	useFile    bool
	filePath   string
	output     string
	f          *os.File
	mu         *sync.Mutex
}

func NewWriter(filePath string) WriterType {
	var mutex sync.Mutex
	w := &Writer{true, false, filePath, "", nil, &mutex}

	if filePath != "" {
		w.useFile = true
		w.f, _ = os.Create(filePath)
	}

	return w
}

func (w *Writer) WriteColor(color string, format string, a ...interface{}) (n int, err error) {
	var formatted = fmt.Sprintf(format, a...)
	w.output += formatted
	if w.useConsole {
		w.mu.Lock()
		if runtime.GOOS == "windows" {
			n, err = fmt.Print(formatted)
		} else {
			n, err = fmt.Print(color + formatted + "\033[0m")
		}
		if w.useFile {
			w.f.WriteString(formatted)
		}
		w.mu.Unlock()
		return n, err
	}

	return n, err
}

func (w *Writer) UseConsole(value bool) {
	w.useConsole = value
}

func (w *Writer) GetOutput() string {
	return w.output
}

func (w *Writer) WriteNewLine() (n int, err error) {
	w.output += "\n"
	if !w.useConsole {
		return
	}
	w.mu.Lock()
	if w.useFile {
		w.f.WriteString("\n")
	}
	n, err = fmt.Println()
	w.mu.Unlock()
	return n, err
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
