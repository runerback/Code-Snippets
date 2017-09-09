using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Runerback.CodeSnippets
{
	public class KeySelectorTest
	{
		public void Do()
		{
			Surface surface = new Surface();

			readDescription(surface, model => model.Data1, data => data.Name);
			readDescription(surface, model => model.Data2, data => data.Value);
		}

		public class Surface
		{
			private DataModel data1 = new DataModel();
			[Description("data model 1")]
			public DataModel Data1
			{
				get { return data1; }
			}

			private DataModel data2 = new DataModel();
			[Description("data model 2")]
			public DataModel Data2
			{
				get { return data2; }
			}

			public class DataModel
			{
				[Description("name of current data model")]
				public string Name { get; set; }

				[Description("value of current data model")]
				public int Value { get; set; }
			}
		}

		private void readDescription<TElement, TModelKey, TFieldKey>(TElement source, Expression<Func<TElement, TModelKey>> modelSelectorExpression, Expression<Func<TModelKey, TFieldKey>> fieldSelectorExpresion)
			where TElement : class
			where TModelKey : class
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (modelSelectorExpression == null)
				throw new ArgumentNullException("modelSelector");
			if (fieldSelectorExpresion == null)
				throw new ArgumentNullException("fieldSelector");

			MemberExpression modelMemberExp = (MemberExpression)modelSelectorExpression.Body;
			var modelAttr = modelMemberExp.Member.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (modelAttr.Length == 0)
				return;
			DescriptionAttribute modelDesc = modelAttr[0] as DescriptionAttribute;
			Console.WriteLine("reading data from [{0}]", modelDesc.Description);

			MemberExpression fieldMemberExp = (MemberExpression)fieldSelectorExpresion.Body;
			var fieldAttr = fieldMemberExp.Member.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (fieldAttr.Length == 0)
				return;
			DescriptionAttribute fieldDesc = fieldAttr[0] as DescriptionAttribute;
			Console.WriteLine("reading [{0}]", fieldDesc.Description);
		}
	}
}
