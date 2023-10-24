import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import AutocompleteInput, { type AutocompleteInputProps } from '../autocomplete-input';

export type AutocompleteFieldProps<
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined,
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & AutocompleteInputProps<T, Multiple, DisableClearable, FreeSolo>;

const AutocompleteField = <
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined,
  TFieldValues extends FieldValues = FieldValues,
>({
  name,
  control,
  ...rest
}: AutocompleteFieldProps<T, Multiple, DisableClearable, FreeSolo, TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field: { onChange, ...restFieldProps }, fieldState: { invalid, error } }) => {
        return (
          <AutocompleteInput
            {...rest}
            {...restFieldProps}
            onChange={(_, value, __, ___) => {
              onChange(value);
            }}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default AutocompleteField;
