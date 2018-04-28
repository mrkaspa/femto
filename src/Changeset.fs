namespace Femto
open System
open System.ComponentModel

module Changeset =
    type Changeset<'T> = {
        data: 'T
        changes: Map<string, string>
        parameters: Map<string, obj>
        errors: Map<string, string>
        valid: bool
        validations: List<ValidateFunc<'T>>
    }
    and ValidateFunc<'T> =
            Changeset<'T> -> Changeset<'T>

    let cast (parameters: Map<string, obj>) (attributes: List<string>) (model: 'T) =
        let filter k _ =
            List.contains k attributes
        let valuesFiltered =
            Map.filter filter parameters

        {
            data = TypeUtils.updateModel valuesFiltered model
            changes = Map.empty
            parameters = parameters
            errors = Map.empty
            valid = false
            validations = []
        }
