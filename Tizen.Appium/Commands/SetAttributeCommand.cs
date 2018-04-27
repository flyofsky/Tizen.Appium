using System;
using Xamarin.Forms;

namespace Tizen.Appium
{
    internal class SetAttributeCommand : ICommand
    {
        public string Command => Commands.SetAttribute;

        public Result Run(Request req)
        {
            Log.Debug("Run: SetAttribute");

            var elementId = req.Params.ElementId;
            var propertyName = req.Params.Attribute;
            var newValue = req.Params.Value;

            var result = new Result();

            var element = ElementUtils.GetElementWrapper(elementId)?.Element;
            if (element == null)
            {
                Log.Debug("Not Found Element");
                return result;
            }

            var property = element.GetType().GetProperty(propertyName);
            if (property == null)
            {
                Log.Debug(elementId + " element does not have " + propertyName + " property.");
                return result;
            }

            try
            {
                if (property.GetValue(element) is Element)
                {
                    var obj = ElementUtils.GetElementWrapper(newValue).Element;
                    if (obj != null)
                    {
                        property.SetValue(element, obj);
                    }
                }
                else
                {
                    var valueType = property.GetValue(element).GetType();
                    var value = Convert.ChangeType(newValue, valueType);
                    Log.Debug(newValue + " is converted to " + value + "(" + value.GetType() + ")");
                    property.SetValue(element, value);
                }

                result.Value = true;
                return result;
            }
            catch (Exception e)
            {
                Log.Debug(e.ToString());
                result.Value = false;
                return result;
            }
        }
    }
}