using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using ItemContext = Xamarin.Forms.Platform.Tizen.Native.ListView.ItemContext;
using EvasObject = ElmSharp.EvasObject;
using System.Threading.Tasks;

namespace Tizen.Appium
{
    public enum ToolbarItemPosition
    {
        None,
        Left,
        Right
    };

    public class FormsElementWrapper : IObject
    {
        WeakReference _element;
        string _id;
        ToolbarItemPosition _position;

        public EventHandler Deleted;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public FormsElementWrapper(object obj)
        {
            Element el;
            if (obj is ItemContext ic)
            {
                el = ic.Cell;
            }
            else
            {
                el = (Element)obj;
            }
            _element = new WeakReference(obj);
            _id = el.GetId();
        }

        public FormsElementWrapper(object obj, ToolbarItemPosition position) : this(obj)
        {
            _position = position;
        }

        public Element Element
        {
            get
            {
                if (_element.IsAlive)
                {
                    if (_element.Target is VisualElement ve)
                    {
                        return (ve.GetIsShownProperty() && ve.IsVisible) ? ve : null;
                    }
                    else if (_element.Target is ItemContext ic)
                    {
                        return ic.Cell.GetIsShownProperty() ? ic.Cell : null;
                    }
                    else if (_element.Target is Element e)
                    {
                        return e;
                    }
                }
                else
                {
                    Deleted?.Invoke(this, EventArgs.Empty);
                }
                return null;
            }
        }

        public EvasObject NativeView
        {
            get
            {
                if (_element.IsAlive)
                {
                    if (_element.Target is VisualElement ve)
                    {
                        return (ve.GetIsShownProperty() && ve.IsVisible) ? Platform.GetRenderer(ve).NativeView : null;
                    }
                    else if (_element.Target is ItemContext ic)
                    {
                        return ic.Cell.GetIsShownProperty() ? ic.Item.TrackObject : null;
                    }
                }
                else
                {
                    Deleted?.Invoke(this, EventArgs.Empty);
                }
                return null;
            }
        }

        public bool Focused
        {
            get
            {
                if (Element is VisualElement ve)
                {
                    return ve.IsFocused;
                }
                return false;
            }
        }

        public Geometry Geometry
        {
            get
            {
                return RunOnMainThread<Geometry>(() =>
                {
                    if (NativeView != null)
                    {
                        return new Geometry(NativeView.Geometry.X, NativeView.Geometry.Y, NativeView.Geometry.Width, NativeView.Geometry.Height);
                    }
                    else
                    {
                        if(_position == ToolbarItemPosition.Right)
                        {
                            var screenWidth = Utils.GetScreeenWidth();
                            return new Geometry(screenWidth - 50, 50, 10, 10);
                        }
                        else if (_position == ToolbarItemPosition.Left)
                        {
                            return new Geometry(50, 50, 10, 10);
                        }
                        else
                        {
                            return new Geometry();
                        }
                    }
                });
            }
        }

        public string Text
        {
            get
            {
                return RunOnMainThread<string>(() =>
                {
                    var text = Element?.GetType().GetProperty("Text")?.GetValue(Element);
                    var name = Element?.GetType().GetProperty("Name")?.GetValue(Element);
                    var formattedText = Element?.GetType().GetProperty("FormattedText")?.GetValue(Element);

                    return (text != null) ? text.ToString() : ((name != null) ? name.ToString() : ((formattedText != null) ? formattedText.ToString() : string.Empty));
                });
            }
        }

        public object GetPropertyValue(string propertyName)
        {
            return RunOnMainThread<object>(() =>
            {
                var value = Element?.GetType().GetProperty(propertyName)?.GetValue(Element);
                return value;
            });
        }

        public bool SetPropertyValue(string propertyName, object value)
        {
            return RunOnMainThread<bool>(() =>
            {
                var property = Element?.GetType().GetProperty(propertyName);
                if (property == null)
                {
                    Log.Debug(Id + " element does not have " + propertyName + " property.");
                    return false;
                }

                try
                {
                    var valueType = property.GetValue(Element).GetType();
                    var convertedValue = Convert.ChangeType(value, valueType);
                    property.SetValue(Element, convertedValue);
                }
                catch (Exception e)
                {
                    Log.Debug(e.ToString());
                    return false;
                }

                return true;
            });
        }

        public bool HasProperty(string propertyName)
        {
            var property = Element?.GetType().GetProperty(propertyName);
            if (property == null)
                return false;

            return true;
        }

        T RunOnMainThread<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Device.BeginInvokeOnMainThread(() =>
            {
                var t = func();
                tcs.SetResult(t);
            });
            return tcs.Task.Result;
        }
    }
}
