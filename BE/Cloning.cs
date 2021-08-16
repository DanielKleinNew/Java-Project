using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	//Implement Deep Cloning using Serializing Deserializing objects
	//https://www.c-sharpcorner.com/UploadFile/ff2f08/deep-copy-of-object-in-C-Sharp/
	public static class Cloning
	{
		public static T Copy <T> (this T source)
		{
			var isNotSerializable = !typeof(T).IsSerializable;
			if(isNotSerializable)
			{
				throw new ArgumentException("this type must be serializable. ", "source");
			}
			var sourceIsNull = ReferenceEquals(source, null);
			if(sourceIsNull)
			{
				return default(T);
			}
			var formatter = new BinaryFormatter();
			using (var stream = new MemoryStream())
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
	}
}
