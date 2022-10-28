# Field Validation

Field validation rules are defined when registering a field using ```useController``` by passing in an array of validation rules.

Ref: https://react-hook-form.com/api/useform/register#options

**Example :**

```fsharp
let firstName =
    useController form.control (fun x -> x.FirstName)
        [ Rules [
              Required "First name is required"
              MinLength(4, "Min length 4")
              MaxLength(50, "Max length 50")
              Pattern("^[A-Z].?", "Must start with capital")
          ] ]
```

## Standard validation

The standard validation rules have been implemented as a pair of rules which receive either:
- A tuple with the error message as the second parameter
- An object matching the Reach-Hook-Form API

The tupled rule is preferred as being more concise and will use the standard name. The object rule will use the name of the rule followed by the ' character.

**Example **

```fsharp
let firstName =
    useController form.control (fun x -> x.FirstName)
        [ Rules [
              Required "First name is required"
              MinLength(4, "Min length 4")
              MaxLength(50, "Max length 50")
              Pattern("^[A-Z].?", "Must start with capital")
          ] ]

let lastName =
    useController form.control (fun x -> x.LaststName)
        [ Rules [
              Required "Last name is required"
              MinLength' { value = 4; message = "Min length 4" }
              MaxLength' { value = 50; message = "Max length 50" }
              Pattern' { value = new Regex("^[A-Z].?"), message = "Must start with capital" }
          ] ]

```



## ```Validate validate```

**validate :** ```'F -> Result<'F, string>```
<br>A function which receives the current value of the field and returns either Ok, if no error, or Error error message.

> `NOTE:` The Ok value will be ignored.

**Example**

```fsharp
let validate v =
    if ...
    then Ok v
    else Error "Not valid"

let fld = useController form.control (fun f -> f.Field)
    [ Rules
        [ Validate validate ]
    ]
```

## ```ValidateAsync validateAsync```

**validateAsync :** ```'F -> Async<Result<'F,string>>```
<br>An async function which receives the current value of the field and returns either Ok, if no error, or Error error message.

> `NOTE:` The Ok value will be ignored.

**Example**

```fsharp
let validateAsync v =
    async {
        let result =
            if ...
            then Ok v
            else Error "Not valid"

        return result
    }

let fld = useController form.control (fun f -> f.Field)
    [ Rules
        [ ValidateAsync validateAsync ]
    ]
```