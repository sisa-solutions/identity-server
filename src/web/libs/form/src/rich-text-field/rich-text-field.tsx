import { Control, Controller, type FieldPath, type FieldValues } from 'react-hook-form';

import RichTextInput, { type RichTextInputProps } from '../rich-text-input';

export type RichtextFieldProps<
  TFieldValues extends FieldValues = FieldValues,
  TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
> = {
  name: TName;
  control: Control<TFieldValues>;
} & RichTextInputProps;

const RichtextField = <TFieldValues extends FieldValues>({
  name,
  control,
  ...rest
}: RichtextFieldProps<TFieldValues>) => {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState: { invalid, error } }) => {
        return (
          // @ts-ignore
          <RichTextInput
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

export default RichtextField;
