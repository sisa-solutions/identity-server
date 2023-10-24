'use client';

import { useRef, useEffect } from 'react';
import {
  init,
  getInstanceByDom,
  type EChartsOption,
  type ECharts,
  type SetOptionOpts,
} from 'echarts';

import Box, { type BoxProps } from '@mui/joy/Box';
import { useColorScheme } from '@mui/joy/styles';

export type ChartProps = Omit<BoxProps, 'children'> & {
  option: EChartsOption;
  settings?: SetOptionOpts;
  loading?: boolean;
  theme?: 'light' | 'dark';
  initOptions?: {
    // locale?: string | LocaleOption;
    renderer?: 'canvas' | 'svg';
    devicePixelRatio?: number;
    useDirtyRect?: boolean;
    useCoarsePointer?: boolean;
    pointerSize?: number;
    ssr?: boolean;
    width?: number | string;
    height?: number | string;
  };
};

const Chart = ({ option, settings, loading, initOptions, sx, ...rest }: ChartProps) => {
  const chartRef = useRef<HTMLDivElement | null>(null);
  const { mode } = useColorScheme();

  useEffect(() => {
    let chart: ECharts | undefined;
    let resizeObserver: ResizeObserver;

    if (chartRef.current !== null) {
      const theme = mode === 'dark' ? 'dark' : 'light';

      chart = init(chartRef.current, theme, initOptions);

      resizeObserver = new ResizeObserver(() => chart?.resize());

      resizeObserver.observe(chartRef.current);
    }

    const refValue = chartRef.current;

    return () => {
      if (resizeObserver) {
        if (refValue) {
          resizeObserver.unobserve(refValue);
        }

        resizeObserver.disconnect();
      }

      chart?.dispose();
    };
  }, [mode, initOptions]);

  useEffect(() => {
    if (chartRef.current !== null) {
      const chart = getInstanceByDom(chartRef.current);

      chart?.setOption(option, settings);
    }
  }, [option, settings, mode]); // Whenever theme changes we need to add option and setting due to it being deleted in cleanup function

  useEffect(() => {
    if (chartRef.current !== null) {
      const chart = getInstanceByDom(chartRef.current);

      loading === true ? chart?.showLoading() : chart?.hideLoading();
    }
  }, [loading, mode]);

  return (
    <Box
      ref={chartRef}
      sx={[
        {
          height: '100%',
          width: '100%',
        },
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...rest}
    />
  );
};

export default Chart;
