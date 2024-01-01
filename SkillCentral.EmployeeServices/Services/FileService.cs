
using AutoMapper;
using SkillCentral.Dtos;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace SkillCentral.EmployeeServices.Services
{
    public class FileService(IMapper mapper, IHttpContextAccessor context, ILogger<FileService> logger) : ServiceBase(context), IFileService
    {
        public async Task<List<T>> ReadEmployeeBatchFile<T>(IFormFile formFile) where T : new()
        {
			try
			{
                List<T> dataList = new List<T>();

                using (var reader = new StreamReader(formFile.OpenReadStream()))
                {
                    // Read the header
                    string[] headers = reader.ReadLine()?.Split(',');

                    while (!reader.EndOfStream)
                    {
                        // Read each line
                        string[] values = reader.ReadLine()?.Split(',');

                        // Skip empty lines
                        if (values == null || values.Length == 0)
                            continue;

                        // Create an object of type T
                        T obj = new T();

                        // Set properties using reflection
                        SetPropertyValues(obj, headers, values);

                        // Add the object to the list
                        dataList.Add(obj);
                    }
                }

                return dataList;
            }
			catch (Exception ex)
			{
                logger.LogError(ex, GlobalConstants.FILE_READING_ERROR);
				throw;
			}
        }

        private void SetPropertyValues<T>(T obj, string[] headers, string[] values)
        {
            Type type = typeof(T);

            for (int i = 0; i < headers.Length && i < values.Length; i++)
            {
                PropertyInfo property = type.GetProperty(headers[i], BindingFlags.Public | BindingFlags.Instance);

                if (property != null && property.CanWrite)
                {
                    Type propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                    // Convert the string value to the property type
                    object typedValue = values[i] == null
                        ? null
                        : Convert.ChangeType(values[i], propertyType);

                    // Set the property value
                    property.SetValue(obj, typedValue);
                }
            }
        }
    }
}
