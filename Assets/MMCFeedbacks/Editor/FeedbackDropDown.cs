using System;
using System.Collections.Generic;
using System.Linq;
using MMCFeedbacks.Core;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MMCFeedbacks.Editor
{
    public class FeedbackDropDown : AdvancedDropdown
    {
        private readonly List<string> _feedbackList = new();
        public event Action<AdvancedDropdownItem> OnSelect;
        
        public FeedbackDropDown(AdvancedDropdownState state) : base(state)
        {
            var types = ReflectionUtility.FindClassesImplementing<IFeedback>();
            var sortedTypes = types.OrderByDescending(i =>
            {
                var instance = Activator.CreateInstance(i);
                if (instance is IFeedback custom) return custom.Order;
                return 0;
            });
            foreach (var type in sortedTypes)
            {
                var instance = Activator.CreateInstance(type);
                if (instance is IFeedback custom) _feedbackList.Add(custom.MenuString);
            }
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem("Feedback");
            foreach (var t in _feedbackList)
            {
                CreateItem(t,root);
            }
            return root;
        }
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            OnSelect?.Invoke(item);
        }
        private void CreateItem(string path,AdvancedDropdownItem root)
        {
            var names = path.Split('/');
            if (names.Length == 1)
            {
                return;
            }
            AdvancedDropdownItem rootElement = null;
            using var e = root.children.GetEnumerator();
            while (e.MoveNext())
                if (names[0] == e.Current?.name)
                    rootElement = e.Current;
            
            string otherName = null;
            for (int i = 1; i < names.Length; i++)
            {
                if (i == 1)
                {
                    otherName = otherName + names[i];
                }
                else
                {
                    otherName = otherName +"/"+ names[i];
                }

            }
            

            if (rootElement != null)
            {
                if (names.Length == 2)
                {
                    var newItem = new AdvancedDropdownItem(names[1]);
                    rootElement.AddChild(newItem);
                }
                else
                {
                    CreateItem(otherName,rootElement);
                }
            }
            else
            {
                var item = new AdvancedDropdownItem(names[0]);
                var newItem = new AdvancedDropdownItem(names[1]);
                if (names[1] != string.Empty) item.AddChild(newItem);
                root.AddChild(item);
                if (names.Length > 2)
                {
                    CreateItem(otherName,item);
                }
            }
        }
    }
}