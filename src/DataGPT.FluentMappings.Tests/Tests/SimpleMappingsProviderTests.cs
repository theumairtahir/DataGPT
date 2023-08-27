using DataGPT.Net.Abstractions.Data;
using DataGPT.Net.Abstractions.Models;
using DataGPT.Net.FluentMappings.Core;
using Moq;

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

	[TestCaseSource(nameof(GetTablesData))]
	public async Task GetEntityMappingsAsync_Returns_MappedDictionary(List<DbTable> tables)
	{
		//arrange
		var mappings = tables.Select(x => x.Name).ToDictionary(x => x);
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetEntityMappingsAsync( );
		//assert
		Assert.That(result, Is.EquivalentTo(mappings));
	}

	[TestCaseSource(nameof(GetTablesData))]
	public void GetColumnsMappings_Returns_MappedColumns(List<DbTable> tables)
	{
		Assert.Multiple(async ( ) =>
		{
			foreach (var table in tables)
			{
				//arrange
				var columnsMappings = table.Columns.Select(x => x.Name).ToDictionary(x => x);
				//act
				var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetColumnMappingsAsync(table.Name);
				//assert
				Assert.That(result, Is.Not.Null);
				Assert.That(columnsMappings, Is.EquivalentTo(result));
			}
		});

	}

	[TestCaseSource(nameof(GetTablesData))]
	public void GetColumnMappingsAsync_ThrowsException_WhenInvalidEntityPassed(List<DbTable> tables)
	{
		Assert.Multiple(( ) =>
		{
			foreach (var table in tables)
			{
				//arrange
				var invalidEntityName = "foo";
				//act
				//assert
				Assert.ThrowsAsync<ArgumentException>(async ( ) => await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetColumnMappingsAsync(invalidEntityName));
			}
		});
	}

	[Test]
	public async Task GetEntityMappingsAsync_ReturnsEmptyDictionary_WhenSchemaIsNull( )
	{
		//arrange
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).ReturnsAsync(( ) => null!);
		var schemaFetcher = schemaFetcherMock.Object;
		//act
		var result = await new SimpleMappingsProvider(schemaFetcher).GetEntityMappingsAsync( );
		//assert
		Assert.Multiple(( ) =>
		{
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.Empty);
		});
	}

	[Test]
	public async Task GetColumnsMappingsAsync_ReturnsEmptyDictionary_WhenEmptyColumns( )
	{
		//arrange
		var tables = new List<DbTable>( )
		{
			new DbTable
			{
				 Name = "foo",
				 ObjectId = "abc"
			}
		};
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetColumnMappingsAsync("foo");
		//assert
		Assert.That(( ) => result, Is.Empty);
	}

	[Test]
	public async Task GetEntityMappingsAsync_ReturnsEmptyDictionary_WhenEmptyColumns( )
	{
		//arrange
		var tables = new List<DbTable>( );
		//act
		var result = await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetEntityMappingsAsync( );
		//assert
		Assert.That(( ) => result, Is.Empty);
	}

	[Test]
	public void GetColumnsMappings_ThrowsArgumentNull_WhenEntityNameIsNull( )
	{
		//arrange
		var tables = new List<DbTable>( );
		string entityName = null;
		//act
		//assert
		Assert.ThrowsAsync<ArgumentNullException>(async ( ) => await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetColumnMappingsAsync(entityName));
	}

	[Test]
	public void GetColumnsMappings_ThrowsArgumentException_WhenSchemaIsEmpty( )
	{
		//arrange
		var tables = new List<DbTable>( );
		string entityName = "foo";
		//act
		//assert
		Assert.ThrowsAsync<ArgumentException>(async ( ) => await new SimpleMappingsProvider(GetSchemaFetcher(tables)).GetColumnMappingsAsync(entityName));
	}

	[Test]
	public async Task SubsequentCallDontTriggerAdditionalCallsTo_GetSchemaAsync( )
	{
		//arrange
		var schemaFetcherMock = new Mock<ISchemaFetcher>( );
		var counter = 0;
		schemaFetcherMock.Setup(x => x.GetSchemaAsync( )).Returns(async ( ) =>
		{
			counter++;
			return await Task.FromResult(new DbSchema( )
			{
				Tables = new List<DbTable>( )
				{
					 new DbTable
					 {
						  Name = "foo",
						   ObjectId="abc"
					 }
				}
			});
		});
		var schemaFetcher = schemaFetcherMock.Object;
		var simpleMappingsProvider = new SimpleMappingsProvider(schemaFetcher);
		//act
		for (int i = 0; i < 10; i++)
		{
			await simpleMappingsProvider.GetEntityMappingsAsync( );
			await simpleMappingsProvider.GetColumnMappingsAsync("foo");
		}
		//assert
		Assert.That(counter, Is.EqualTo(1));
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

	private static List<DbTable>[ ] GetTablesData( )
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
