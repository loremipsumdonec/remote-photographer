using RemotePhotographer.Features.Auto.Models;

namespace RemotePhotographer.Features.Auto.Schema;

public class SessionInputSchemaType
    : InputObjectType<Session>
{
    protected override void Configure(IInputObjectTypeDescriptor<Session> descriptor)
    {
        descriptor.Field("actions").Type<ListType<CaptureImageActionSchemaType>>();
    }
}