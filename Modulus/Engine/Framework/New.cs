﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Engine.Framework
{
    public static class New<T> where T : new()
    {
        public static T Instantiate {
            get
            {
                return instance();
            }
                
        }
        private static Func<T> instance = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
        public static void Override<R>()
        {
            instance = Expression.Lambda<Func<T>>(Expression.New(typeof(R))).Compile();
        }
    }
}
