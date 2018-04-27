namespace Femto

module Changeset =
    type Changeset<'T> = {
        data: 'T
        changes: 'T
        parameteres: Map<string, string>
        errors: Map<string, string>
        valid: bool
        validations: List<ValidateFunc<'T>>
    }
    and ValidateFunc<'T> =
            Changeset<'T> -> Changeset<'T>

    let cast model parameters attributes =
        let filter k v =
            k in attributes
        let valuesFiltered =
            Map.filter filter parameters
        ()
