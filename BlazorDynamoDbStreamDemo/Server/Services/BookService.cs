using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using BlazorDynamoDbStreamDemo.Shared;
using Grpc.Core;

namespace BlazorDynamoDbStreamDemo.Server.Services;

public class BookService : Shared.BookService.BookServiceBase
{
	private readonly AmazonDynamoDBClient _amazonDynamoDbClient;

	public BookService(AmazonDynamoDBClient amazonDynamoDbClient)
	{
		_amazonDynamoDbClient = amazonDynamoDbClient;
	}

	public override async Task GetBooks(GetBooksRequest request, IServerStreamWriter<GetBooksReply> responseStream, ServerCallContext context)
	{
		var table = Table.LoadTable(_amazonDynamoDbClient, "Books");

		var search = table.Query(new QueryOperationConfig
		{
			ConsistentRead = true,
			Filter = new QueryFilter("Pk", QueryOperator.Equal, 1),
			Limit = request.PageSize,
			Select = SelectValues.AllAttributes
		});

		do
		{
			var items = await search.GetNextSetAsync();

			foreach (var item in items)
			{
				await responseStream.WriteAsync(new GetBooksReply
				{
					Pk = item["Pk"].AsInt(),
					Sk = item["Sk"].AsInt(),
					Title = item["Title"]
				});
			}
		}
		while (!search.IsDone && !context.CancellationToken.IsCancellationRequested);
	}
}
