using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarController : MonoBehaviour
{
    public DatePicker datePicker;
    // Start is called before the first frame update
    void Start()
    {
        datePicker.SetSelectedDate(System.DateTime.Today);
        datePicker.GenerateDaysNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
