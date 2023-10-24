import { forwardRef, useRef } from 'react';

import {dayUtils} from '@sisa/i18n';

import {
  CaptionProps,
  DayPicker,
  useNavigation,
  useInput,
  useDayRender,
  DayProps,
  useDayPicker,
} from 'react-day-picker';

import Box from '@mui/joy/Box';
import Stack from '@mui/joy/Stack';
import Typography from '@mui/joy/Typography';

import FormControl from '@mui/joy/FormControl';
import FormLabel from '@mui/joy/FormLabel';
import FormHelperText from '@mui/joy/FormHelperText';
import Input, { type InputProps } from '@mui/joy/Input';

import Dropdown from '@mui/joy/Dropdown';
import Menu from '@mui/joy/Menu';
import MenuButton from '@mui/joy/MenuButton';

import Card from '@mui/joy/Card';
import CardContent from '@mui/joy/CardContent';
import IconButton from '@mui/joy/IconButton';

import { ChevronDownIcon, ChevronLeftIcon, ChevronRightIcon } from 'lucide-react';

export type DateInputProps = InputProps & {
  label?: React.ReactNode;
  helperMessage?: React.ReactNode;
  errorMessage?: React.ReactNode;
};

const Caption = ({ displayMonth }: CaptionProps) => {
  const { numberOfMonths } = useDayPicker();
  const { goToMonth, nextMonth, previousMonth, displayMonths } = useNavigation();

  const displayIndex = displayMonths.findIndex(
    (month) => displayMonth.getMonth() == month.getMonth()
  );

  const isFirst = displayIndex === 0;
  const isLast = displayIndex === displayMonths.length - 1;

  const hidePrevious = numberOfMonths > 1 && (isLast || !isFirst);
  const hideNext = numberOfMonths > 1 && (isFirst || !isLast);

  return (
    <Stack
      direction="row"
      alignItems="center"
      sx={{
        mb: 1,
      }}
    >
      {!hidePrevious && (
        <IconButton
          disabled={!previousMonth}
          onClick={() => previousMonth && goToMonth(previousMonth)}
        >
          <ChevronLeftIcon />
        </IconButton>
      )}
      <Typography level="title-lg" sx={{ flex: 1, textAlign: 'center' }}>
        {dayUtils(displayMonth).format('MMM yyy')}
      </Typography>
      {!hideNext && (
        <IconButton disabled={!nextMonth} onClick={() => nextMonth && goToMonth(nextMonth)}>
          <ChevronRightIcon />
        </IconButton>
      )}
    </Stack>
  );
};

const Day = ({ date, displayMonth }: DayProps) => {
  const buttonRef = useRef<HTMLButtonElement>(null);
  const dayRender = useDayRender(date, displayMonth, buttonRef);

  if (dayRender.isHidden) {
    return <Box role="gridcell" />;
  }

  if (!dayRender.isButton) {
    const { children, ...restDivProps } = dayRender.divProps;

    return (
      <IconButton
        sx={{
          borderRadius: '50%',
        }}
        slotProps={{
          root: {
            component: Box,
            ...restDivProps,
          },
        }}
      >
        {children}
      </IconButton>
    );
  }

  return (
    <IconButton
      ref={buttonRef}
      sx={{
        borderRadius: '50%',
      }}
      {...dayRender.buttonProps}
    />
  );
};

const DateInput = forwardRef<HTMLDivElement, DateInputProps>(
  (
    { label, helperMessage, errorMessage, required, error, disabled, size, sx, ...restProps },
    ref
  ) => {
    const {
      inputProps,
      dayPickerProps: { ...restDayPickerProps },
    } = useInput({
      defaultSelected: new Date(),
      format: 'PPP',
      required,
    });

    return (
      <FormControl required={required} error={error} disabled={disabled} sx={sx} size={size}>
        <Box>
          {label && <FormLabel>{label}</FormLabel>}
          {helperMessage && <Typography level="body-sm">{helperMessage}</Typography>}
        </Box>
        <Dropdown>
          <Input
            ref={ref}
            slotProps={{
              input: {
                ...inputProps,
              },
            }}
            endDecorator={
              <MenuButton slots={{ root: IconButton }}>
                <ChevronDownIcon />
              </MenuButton>
            }
            {...restProps}
          />
          <Menu
            component={Card}
            sx={{
              p: 2,

              '& .rdp-months': {
                display: 'flex',
                gap: 4,
                flexDirection: 'row',
              },

              '& .rdp-day_outside': {
                color: 'text.tertiary',
              },

              '& .rdp': {
                '--rdp-outline': {
                  color: 'red',
                },

                '--rdp-outline-selected': {
                  color: 'red',
                },
              },
            }}
          >
            <CardContent
              component={DayPicker}
              components={{
                Caption,
                Day,
              }}
              pagedNavigation
              fixedWeeks
              showOutsideDays
              {...restDayPickerProps}
            />
          </Menu>
          {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
        </Dropdown>
      </FormControl>
    );
  }
);

DateInput.displayName = 'DateInput';

export default DateInput;
