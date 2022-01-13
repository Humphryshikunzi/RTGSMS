/*
 * L70R.c
 *
 * L70R gps module functions
 * Warefab Konnect STM32L0-SIGFOX Development Board
 *
 * Created on: Nov 8, 2019
 * Author: Muchiri John
 * (c) wwww.warefab.com
 *
 * This software component is licensed by warefab under BSD 3-Clause license,
 * the "License"; You may not use this file except in compliance with the
 * License. You may obtain a copy of the License at:
 *
 * opensource.org/licenses/BSD-3-Clause
 */

#include <string.h>
#include <stdio.h>
#include <stdlib.h>

#include "L70R.h"

/*
 * init gps
 */
void l70_init() {
	l70 = &l70_info;

	l70->time = 0;
	l70->lat = 0;
	l70->lat_ns = 0;
	l70->lng = 0;
	l70->lng_ew = 0;
	l70->speed = 0;
	l70->date = 0;
}

/*
 * put gps in standby mode
 */
void l70_standby(uint8_t flag) {

	__HAL_UART_DISABLE_IT(&huart2, UART_IT_RXNE);
	if (flag == 1) {
		usart_puts(&huart2, (char*) L70_STDY);
	}
	usart_puts(&huart2, (char*) "\r\n");
	__HAL_UART_ENABLE_IT(&huart2, UART_IT_RXNE);

	//l70_init();
}

/*
 * gps nmea sentence output mode
 */
void l70_mode(uint8_t mode) {
	//__HAL_UART_DISABLE_IT(&huart2, UART_IT_RXNE);
	if (mode == 0) {
		usart_puts(&huart2, (char*) NMEA_NONE);
	} else if (mode == 1) {
		usart_puts(&huart2, (char*) RMC_1_PFIX);
	} else if (mode == 2) {
		usart_puts(&huart2, (char*) NMEA_ALL);
	}
	usart_puts(&huart2, (char*)"\r\n");
	//__HAL_UART_ENABLE_IT(&huart2, UART_IT_RXNE);
}
/*
 * parse gps nmea sentence, GPRMC
 */
uint8_t l70_parse(uint8_t *nmea, uint16_t len) {
	uint16_t size = len;
	double result;
	buf_cur_pos = 0;
	uint8_t x = 0;
	uint8_t y = 0;

	char buf[12] = { 0 };
	//char ut[40];

	p_gps = &gpsbuf[0];

	for (size = 0; size < len; size++) {
		*(p_gps + size) = *(nmea + size);
	}
	*(p_gps + size + 1) = 0;

	if (strstr((char*) gpsbuf, "$GPRMC") && strstr((char*) gpsbuf, "*")) {
		/*usart_puts(&huart1, (char*) gpsbuf);
		 usart_puts(&huart1, (char*) "\r\n");*/

		//strtok((char*) gpsbuf, ","); //nmea msg
		//p_gps = &gpsbuf[0];
		for (size = 0; size < len; size++) {
			if (*(p_gps + size) == ',') {
				if (x > 2) {
					result = strtod(buf, NULL);
				} else {
					result = 0.00;
				}
				//sprintf(ut, "%d -> %s, %lf\r\n", y, buf, result);
				//usart_puts(&huart1, (char*) ut);
				switch (y) {
				case 0: //nmea - gprmc
				case 2: //validity
				case 4: //lat dir
					l70->lat_ns = buf[0];
				case 6: //lng dir
					l70->lng_ew = buf[0];
				case 8: //magnetic variation
					break;
				case 1: //time
					l70->time = ((uint32_t) result) + 30000;
					if (l70->time > 240000)
						l70->time = l70->time - 240000;
					break;
				case 3: //latitude
					l70->lat = convertRawCoords((uint32_t) (result * 10000.0));
					break;
				case 5: //longitude
					l70->lng = convertRawCoords((uint32_t) (result * 10000.0));
					break;
				case 7: //ground course speed km/hr
					l70->speed = (uint8_t) (result * 1.852); // * 100.0);
					break;
				case 9: //date
					l70->date = (uint32_t) result;
					break;
				default:
					break;
				};

				if (y >= 9)
					return 1;

				memset(buf, 0, 12);
				x = 0;
				y++;
				//size++;
			} else {
				buf[x++] = *(p_gps + size); //*p_gps++;
			}
		}
	}
	return 0;
}

/*
 * convert raw gps coords dd.mmmm to dd.dddd
 */
uint32_t convertRawCoords(uint32_t coord) {
	uint16_t crd_d1 = coord / 1000000;
	uint32_t crd_d2 = ((coord % 1000000) * 10) / 60;
	crd_d2 = (crd_d1 * 100000) + crd_d2;
	return crd_d2;
}
