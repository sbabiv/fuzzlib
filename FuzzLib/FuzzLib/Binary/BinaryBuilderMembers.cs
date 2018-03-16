using System;
using System.Collections.Generic;
using System.Reflection;

using FuzzLib.Functions;

namespace FuzzLib.Binary
{
    public class BinaryBuilderMembers
    {
        public Action<object, MethodInfo, string> AddHandler;
        public Action ClearHanlder;
        public Func<Dictionary<string, object>, string> RenderHanlder;

        public IFunctionsContainer FunctionsContainer;

        public BinaryBuilderMembers(Action<object, MethodInfo, string> addHandler, Action clearHanlder, Func<Dictionary<string, object>, string> renderHanlder, IFunctionsContainer functionsContainer)
        {
            AddHandler = addHandler;
            ClearHanlder = clearHanlder;
            RenderHanlder = renderHanlder;
            FunctionsContainer = functionsContainer;
        }
    }
}
