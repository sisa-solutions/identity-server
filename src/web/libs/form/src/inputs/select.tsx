import BaseSelect, { type SelectProps } from '@mui/joy/Select';

import { ChevronDownIcon } from 'lucide-react';

const Select = <TValue extends {}>(props: SelectProps<TValue>) => {
  return <BaseSelect indicator={<ChevronDownIcon />} {...props} />;
};

export default Select;
