using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Fills a calendar using CalendarDateItem
/// </summary>
public class CalendarController : MonoBehaviour
{
    public Text _yearNumText;
    public Text _monthNumText;
    public RectTransform casesParent;

    public GameObject empty;
    public GameObject _item;

    public List<GameObject> empties = new List<GameObject>();
    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController _calendarInstance;

    void Start()
    {
        float width = casesParent.rect.width;
        float height = casesParent.rect.height;
        Vector2 newSize = new Vector2(width / 7, height / 6);
        casesParent.GetComponent<GridLayoutGroup>().cellSize = newSize;

        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(casesParent);
            item.transform.localScale = Vector3.one;

            _dateItems.Add(item);
        }
        for (int i = 0; i < 7; i++)
        {
            GameObject e = Instantiate(empty, casesParent);
            e.transform.SetAsFirstSibling();
            empties.Add(e);
        }
        _dateTime = DateTime.Now;
        CreateCalendar();
    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);
        for (int i = 0; i < empties.Count; i++)
        {
            empties[i].SetActive(i < index);
        }

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);
                    //Debug.Log(thatDay.ToShortDateString());

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString();
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }
}
