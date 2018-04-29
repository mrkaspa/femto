namespace Femto

module Changeset =
    type ValidateFunc<'T> = 'T -> bool

    type Changeset<'T> = {
        data: 'T
        changes: Map<string, obj>
        parameters: Map<string, obj>
        errors: Map<string, string>
        valid: Option<bool>
        validations: List<ValidateFunc<'T>>
    }

    let cast (parameters: Map<string, obj>) (attributes: List<string>) (model: 'T) =
        let filter k _ =
            List.contains k attributes
        let valuesFiltered =
            Map.filter filter parameters
        {
            data = TypeUtils.updateModel valuesFiltered model
            changes = valuesFiltered
            parameters = parameters
            errors = Map.empty
            valid = None
            validations = []
        }

    let validate changeset =
        let valid =
            changeset.validations
            |> List.fold
                (fun valid validation ->
                    if valid then validation changeset.data
                    else valid)
                false
        { changeset with valid = Some valid }
