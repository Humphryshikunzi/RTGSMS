################################################################################
# Automatically-generated file. Do not edit!
# Toolchain: GNU Tools for STM32 (9-2020-q2-update)
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS += \
../Src/L70R.c \
../Src/LIS2DH12.c \
../Src/W25Q64JV.c \
../Src/adc.c \
../Src/buzzer.c \
../Src/dma.c \
../Src/gpio.c \
../Src/i2c.c \
../Src/main.c \
../Src/sht30x.c \
../Src/sigfox.c \
../Src/spi.c \
../Src/stm32l0xx_hal_msp.c \
../Src/stm32l0xx_it.c \
../Src/sys.c \
../Src/syscalls.c \
../Src/sysmem.c \
../Src/system_stm32l0xx.c \
../Src/tim.c \
../Src/usart.c \
../Src/utils.c 

OBJS += \
./Src/L70R.o \
./Src/LIS2DH12.o \
./Src/W25Q64JV.o \
./Src/adc.o \
./Src/buzzer.o \
./Src/dma.o \
./Src/gpio.o \
./Src/i2c.o \
./Src/main.o \
./Src/sht30x.o \
./Src/sigfox.o \
./Src/spi.o \
./Src/stm32l0xx_hal_msp.o \
./Src/stm32l0xx_it.o \
./Src/sys.o \
./Src/syscalls.o \
./Src/sysmem.o \
./Src/system_stm32l0xx.o \
./Src/tim.o \
./Src/usart.o \
./Src/utils.o 

C_DEPS += \
./Src/L70R.d \
./Src/LIS2DH12.d \
./Src/W25Q64JV.d \
./Src/adc.d \
./Src/buzzer.d \
./Src/dma.d \
./Src/gpio.d \
./Src/i2c.d \
./Src/main.d \
./Src/sht30x.d \
./Src/sigfox.d \
./Src/spi.d \
./Src/stm32l0xx_hal_msp.d \
./Src/stm32l0xx_it.d \
./Src/sys.d \
./Src/syscalls.d \
./Src/sysmem.d \
./Src/system_stm32l0xx.d \
./Src/tim.d \
./Src/usart.d \
./Src/utils.d 


# Each subdirectory must supply rules for building sources it contributes
Src/%.o: ../Src/%.c Src/subdir.mk
	arm-none-eabi-gcc "$<" -mcpu=cortex-m0plus -std=gnu11 -g3 -DUSE_HAL_DRIVER -DSTM32L052xx -DDEBUG -c -I../Inc -I../Drivers/CMSIS/Include -I../Drivers/STM32L0xx_HAL_Driver/Inc -I../Drivers/CMSIS/Device/ST/STM32L0xx/Include -I../Drivers/STM32L0xx_HAL_Driver/Inc/Legacy -O3 -ffunction-sections -fdata-sections -Wall -fstack-usage -MMD -MP -MF"$(@:%.o=%.d)" -MT"$@" --specs=nano.specs -mfloat-abi=soft -mthumb -o "$@"

