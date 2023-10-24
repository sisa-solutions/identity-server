import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import NumericFormatInput, { type NumericFormatInputProps } from '../numeric-format-input';

type NumberFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & NumericFormatInputProps;

const NumericFormatField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: NumberFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <NumericFormatInput
            {...rest}
            {...field}
            error={!!error || invalid}
            errorMessage={error?.message}
          />
        );
      }}
    />
  );
};

export default NumericFormatField;
