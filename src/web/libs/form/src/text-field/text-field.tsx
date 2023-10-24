import { type Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import TextInput, { type TextInputProps } from '../text-input';

type TextFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & TextInputProps;

const TextField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: TextFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          <TextInput
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

export default TextField;
