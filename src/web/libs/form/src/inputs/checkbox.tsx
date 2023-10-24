import BaseCheckbox, { type CheckboxProps } from '@mui/joy/Checkbox';
import { CheckIcon, MinusIcon } from 'lucide-react';

const Checkbox = (props: CheckboxProps) => {
  return <BaseCheckbox checkedIcon={<CheckIcon />} indeterminateIcon={<MinusIcon />} {...props} />;
};

export default Checkbox;
