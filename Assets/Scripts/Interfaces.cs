using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ButtonHolder
{
    void ButtonPressed(int b);

    bool IsResponsive();
}

public interface DialogHolder
{
    void DialogClosed(int tag);
    void DialogOK(int tag);

    void ShowDialogInfo(string s);

    void ShowDialogQuestion(string s, int tag);
}
