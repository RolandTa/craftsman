﻿namespace Craftsman.Builders.Endpoints
{
    using System.Collections.Generic;
    using Enums;
    using Helpers;
    using Models;

    public class PatchRecordEndpointBuilder
    {
        public static string GetEndpointTextForPatchRecord(Entity entity, bool addSwaggerComments,List<Policy> policies)
        {
            var lowercasePrimaryKey = Entity.PrimaryKeyProperty.Name.LowercaseFirstLetter();
            var entityName = entity.Name;
            var updateDto = Utilities.GetDtoName(entityName, Dto.Update);
            var primaryKeyProp = Entity.PrimaryKeyProperty;
            var patchRecordCommandMethodName = Utilities.CommandPatchName(entityName);
            var pkPropertyType = primaryKeyProp.Type;
            var updatePartialAuthorizations = EndpointSwaggerCommentBuilders.BuildAuthorizations(policies);

            return @$"{EndpointSwaggerCommentBuilders.GetSwaggerComments_PatchRecord(entity, addSwaggerComments, updatePartialAuthorizations.Length > 0)}{updatePartialAuthorizations}
    [Consumes(""application/json"")]
    [Produces(""application/json"")]
    [HttpPatch(""{{{lowercasePrimaryKey}:{primaryKeyProp.Type.ToLower()}}}"", Name = ""PartiallyUpdate{entityName}"")]
    public async Task<IActionResult> PartiallyUpdate{entityName}({pkPropertyType} {lowercasePrimaryKey}, JsonPatchDocument<{updateDto}> patchDoc)
    {{
        var command = new {Utilities.PatchEntityFeatureClassName(entity.Name)}.{patchRecordCommandMethodName}({lowercasePrimaryKey}, patchDoc);
        await _mediator.Send(command);

        return NoContent();
    }}";
        }
    }
}