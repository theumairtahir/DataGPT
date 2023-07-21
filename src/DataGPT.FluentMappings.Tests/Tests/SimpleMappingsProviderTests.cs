using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Models;
using Moq;
using DataGPT.Net.FluentMappings.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGPT.Net.FluentMappings.Tests.Tests;

[TestFixture]
internal class SimpleMappingsProviderTests
{
	[Test]
	public void Object_CreatesSuccessfully( )
	{
		//arrange
		var tables = new List<DbTable>
			{
				 new DbTable
				 {
					Name = "Students",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "StudentName",
						},
						new DbColumn
						{
							DataType = "DateTime",
							Name = "DOB",
						},
					}
				 },
			};
		SimpleMappingsProvider simpleMappingsProvider;
		//act
		simpleMappingsProvider = new SimpleMappingsProvider(GetSchemaFetcher(tables));
		//assert
		Assert.That(simpleMappingsProvider, Is.Not.Null);
	}

	[Test]
	public async Task GetEntityMappingsAsync_Returns_MappedDictionary( )
	{
		//arrange
		var tables = new List<DbTable>
			{
				 new DbTable
				 {
					Name = "Students",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "StudentName",
						},
						new DbColumn
						{
							DataType = "DateTime",
							Name = "DOB",
						},
					}
				 },
				new DbTable
				 {
					Name = "Courses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "CourseName",
						},
					}
				 },
				new DbTable
				 {
					Name = "StudentsCourses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "PrimaryKey",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
					}
				 },
			};
		var mappings = tables.Select(x => x.Name).ToDictionary(x => x);
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetEntityMappingsAsync( );
		//assert
		Assert.That(result, Is.EquivalentTo(mappings));
	}


	private static ISchemaFetcher GetSchemaFetcher(List<DbTable> tables)
	{
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).Returns(async ( ) => await Task.FromResult(new DbSchema( )
		{
			Tables = tables
		}));
		return schemaFetcherMock.Object;
	}

	public static List<DbTable>[ ] GetTablesData( )
	{
		return new List<DbTable>[ ]
		{
			new List<DbTable>
			{
				 new DbTable
				 {
					Name = "Students",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "StudentName",
						},
						new DbColumn
						{
							DataType = "DateTime",
							Name = "DOB",
						},
					}
				 },
				new DbTable
				 {
					Name = "Courses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
						new DbColumn
						{
							DataType = "Varchar",
							Name = "CourseName",
						},
					}
				 },
				new DbTable
				 {
					Name = "StudentsCourses",
					ObjectId = Guid.NewGuid().ToString(),
					IsView = false,
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							DataType = "Integer",
							Name = "PrimaryKey",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "StudentId",
						},
						new DbColumn
						{
							DataType = "Integer",
							Name = "CourseId",
						},
					}
				 },
			},
			new List<DbTable>(),
			new List<DbTable>
			{
			   new DbTable
			   {
				    Name = "Product",
					 ObjectId= Guid.NewGuid().ToString(),
			   },
			   new DbTable
			   {
				    Name = "Category",
					ObjectId= Guid.NewGuid().ToString(),
					Columns = new List<DbColumn>
					{
						new DbColumn
						{
							 DataType = "Integer",
							  Name= "CategoryId",
						},
						new DbColumn
						{
							 DataType = "Varchar",
							  Name= "CategoryName",
						},
					}
			   }
			}
		};
	}
}
