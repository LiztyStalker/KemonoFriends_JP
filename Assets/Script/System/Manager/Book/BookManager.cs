using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BookManager : SingletonClass<BookManager>
{


    public int ElementCount
    {
        get { return 0; }
    }

    public BookManager()
    {
        initParse();
    }

    void initParse()
    {
    }
}

