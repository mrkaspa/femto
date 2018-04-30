namespace Femto

module Changeset =

    type Validation<'T> = {
        func: ValidateFunc<'T>
        field: FieldName
        errMsg: ErrMessage
    }
    and FieldName = FieldName of string
    and ErrMessage = ErrMessage of string
    and ValidateFunc<'T> = 'T -> bool

    type Changeset<'T> = {
        data: 'T
        changes: Map<string, obj>
        parameters: Map<string, obj>
        errors: Map<string, string>
        valid: Option<bool>
        validations: List<Validation<'T>>
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

    let addValidation func fieldName errMsg changeset =
        let validation = {
            func = func
            field = FieldName fieldName
            errMsg = ErrMessage errMsg
        }

        { changeset with validations = validation :: changeset.validations }

    let validate changeset =
        let foldFunc
            (valid, errors)
            {func = func; field = FieldName fieldName; errMsg = ErrMessage errMsg} =
            if func changeset.data then
                (false, Map.add fieldName errMsg errors)
            else
                (valid, errors)

        let (valid, errors) =
            changeset.validations
            |> List.fold foldFunc (true, Map.empty)

        { changeset with valid = Some valid; errors = errors }
