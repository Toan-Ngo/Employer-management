import { Injectable } from '@angular/core';
import { ChartOptions, ChartType, ScaleOptions } from 'chart.js';
import { getStyle } from '@coreui/utils';

export interface IChartProps {
  data?: any;
  labels?: any;
  options?: ChartOptions;
  colors?: any;
  type: ChartType;
  legend?: any;
  [propName: string]: any;
}

@Injectable({
  providedIn: 'any'
})
export class WorkforceOverviewChartsData {

  constructor() {}

  getScales(): ScaleOptions<any> {

    const colorBorderTranslucent = getStyle('--cui-border-color-translucent');
    const colorBody = getStyle('--cui-body-color');

    return {
      x: {
        grid: {
          color: colorBorderTranslucent,
          drawOnChartArea: false
        },
        ticks: {
          color: colorBody
        }
      },
      y: {
        beginAtZero: true,
        grid: {
          color: colorBorderTranslucent
        },
        ticks: {
          color: colorBody,
          precision: 0
        }
      }
    };
  }

}