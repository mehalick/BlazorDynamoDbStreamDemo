using BlazorDynamoDbStreamDemo.Shared;
using Grpc.Core;

namespace BlazorDynamoDbStreamDemo.Server.Services;

public class BookService : Shared.BookService.BookServiceBase
{
	public override async Task GetBooks(GetBooksRequest request, IServerStreamWriter<GetBooksReply> responseStream, ServerCallContext context)
	{
		throw new NotImplementedException();
	}
}
