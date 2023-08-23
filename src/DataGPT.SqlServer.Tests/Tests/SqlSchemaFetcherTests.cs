using Dapper;
using DataGPT.Net.Abstractions.Models;
using DataGPT.Net.SqlServer.Core;
using DataGPT.Net.SqlServer.Types.Models;
using Moq;
using Moq.Dapper;
using System.Data;

namespace DataGPT.Net.SqlServer.Tests.Tests;

[TestFixture]
internal class SqlSchemaFetcherTests
{
	private const string SQL_QUERY_REGEX = "SELECT\\s.*FROM\\s.*WHERE\\s.*";


	[Test]
	public void Object_CreatesSuccessfully( )
	{
		//arrange
		SqlSchemaFetcher sqlSchemaFetcher;
		//act
		sqlSchemaFetcher = new SqlSchemaFetcher(GetFakeConnection( ));
		//assert
		Assert.That(sqlSchemaFetcher, Is.Not.Null);
	}

	[Test]
	public async Task GetSchemaAsync_FetchesProperResults( )
	{
		//arrange
		var expectedResult = new DbSchema { Tables = GetColumnsData( ).GroupBy(x => (x.TableName, x.ObjectId, x.IsView)).Select(t => new DbTable { Name = t.Key.TableName, ObjectId = t.Key.ObjectId.ToString( ), IsView = t.Key.IsView, Columns = t.Select(c => new DbColumn { DataType = c.DataType, Name = c.ColumnName }).ToList( ) }).ToList( ) };
		//act
		var result = await new SqlSchemaFetcher(GetFakeConnection( )).GetSchemaAsync( );
		//assert
		Assert.Multiple(( ) =>
		{
			Assert.That(result.Tables, Is.EquivalentTo(expectedResult.Tables));
			foreach (var table in result.Tables)
			{
				Assert.That(table.Columns, Is.EquivalentTo(expectedResult.Tables.FirstOrDefault(x => x.Name == table.Name)?.Columns));
			}
		});
	}

	[Test]
	public async Task GetSchemaAsync_ReturnsEmptyDbSchema_WhenNoResultsFromDb( )
	{
		//arrange
		var connectionFaker = new Mock<IDbConnection>( );

		connectionFaker.SetupDapperAsync(c => c.QueryAsync<ColumnSqlResult>(It.IsRegex(SQL_QUERY_REGEX), null, null, null, null)).ReturnsAsync(Enumerable.Empty<ColumnSqlResult>( ));

		var connection = connectionFaker.Object;
		//act
		var result = await new SqlSchemaFetcher(connection).GetSchemaAsync( );
		//assert
		Assert.That(result.Tables, Is.Empty);
	}

	//[Test]
	//public void GetSchemaAsync_ThrowsException_WhenDbError( )
	//{
	//	//arrange
	//	var connectionFaker = new Mock<IDbConnection>( );

	//	connectionFaker.SetupDapperAsync(c => c.QueryAsync<ColumnSqlResult>(It.IsRegex(SQL_QUERY_REGEX), null, null, null, null)).ThrowsAsync(new Mock<SqlException>( ).Object);

	//	var connection = connectionFaker.Object;
	//	//act
	//	//assert
	//	Assert.ThrowsAsync<IncompleteDbActionException>(async ( ) => await new SqlSchemaFetcher(connection).GetSchemaAsync( ));
	//}

	private static IDbConnection GetFakeConnection( )
	{
		var connection = new Mock<IDbConnection>( );

		connection.SetupDapperAsync(c => c.QueryAsync<ColumnSqlResult>(It.IsRegex(SQL_QUERY_REGEX), null, null, null, null)).ReturnsAsync(GetColumnsData( ));

		return connection.Object;
	}

	private static List<ColumnSqlResult> GetColumnsData( )
	{
		return new List<ColumnSqlResult>
			{
				 new ColumnSqlResult
				 {
					DataType = "Integer",
					ColumnName = "StudentId",
					TableName = "Students",
					ObjectId = 1,
					IsView = false
				 },
				new ColumnSqlResult
				{
					DataType = "Varchar",
					ColumnName = "StudentName",
					TableName = "Students",
					ObjectId = 1,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "DateTime",
					ColumnName = "DOB",
					TableName = "Students",
					ObjectId = 1,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Integer",
					ColumnName = "CourseId",
					TableName = "Courses",
					ObjectId = 2,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Varchar",
					ColumnName = "CourseName",
					TableName = "Courses",
					ObjectId = 2,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Integer",
					ColumnName = "PrimaryKey",
					TableName = "StudentsCourses",
					ObjectId = 3,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Integer",
					ColumnName = "StudentId",
					TableName = "StudentsCourses",
					ObjectId = 3,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Integer",
					ColumnName = "CourseId",
					TableName = "StudentsCourses",
					ObjectId = 3,
					IsView = false
				},
				new ColumnSqlResult
				{
					DataType = "Integer",
					ColumnName= "CategoryId",
					TableName = "Category",
					ObjectId = 4,
					IsView = true
				},
				new ColumnSqlResult
				{
					DataType = "Varchar",
					ColumnName= "CategoryName",
					TableName = "Category",
					ObjectId = 4,
					IsView = true
				}
			};
	}
}
