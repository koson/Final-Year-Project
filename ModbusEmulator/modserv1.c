/*
 * Copyright © 2008-2010 Stéphane Raimbault <stephane.raimbault@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.

 *********************************************************************
 * 2013-2014
 * Heavily modified by Ron Ostafichuk for 'Modbus on The Pi' web article
 *
 ************************************************************************
 * 2017-2018
 * Further modified by Raife White for University of Portsmouth Final Year Project
 */
#include <stdio.h>
#include <unistd.h>
#include <string.h>
#include <stdlib.h>
#include <errno.h>
#include <time.h>
#include <modbus.h>
#include <math.h>
// WAY easier method to access Pi IO functions!  Thanks to WiringPi.com!
#include <wiringPi.h>

enum {
    TCP,
    RTU
};

int main(int argc, char *argv[])
{
    int socket;
    modbus_t *ctx;
    modbus_mapping_t *mb_mapping;
    int rc;
    int use_backend;
    int nPort = 1500; // default for single instance
    int nPin4Value = 0; // for reading Pin4 switch value

    // allow user override of port number, this is done so I can run multiple instances on different ports with a bash script
    if( argc > 1 )
    {
	nPort = atoi(argv[2]);
    }
    
    // to emulate a MODBUS device we need at least 8 input and holding registers
    mb_mapping = modbus_mapping_new(8, 8, 80, 80);
    if (mb_mapping == NULL) {
        fprintf(stderr, "Failed to allocate the mapping: %s\n", modbus_strerror(errno));
        modbus_free(ctx);
        return -1;
    }
    
     /* TCP */
    if (argc > 1) {
        if (strcmp(argv[1], "tcp") == 0) {
            use_backend = TCP;
        } else if (strcmp(argv[1], "rtu") == 0) {
            use_backend = RTU;
        } else {
            printf("Usage:\n  %s [tcp Port|rtu] - Modbus client for testing a server\n\n", argv[0]);
            exit(1);
        }
    } else {
        /* By default */
        use_backend = TCP;
	printf("modserv:\n Running in tcp mode - Modbus client for testing a server\n\n");
    }

    if (use_backend == TCP) {
        printf("Waiting for TCP connection on Port %i \n",nPort);
        ctx = modbus_new_tcp("127.0.0.1", nPort);
        socket = modbus_tcp_listen(ctx, 1);
        modbus_tcp_accept(ctx, &socket);
	printf("TCP connection started!\n");
    } else {
	printf("Waiting for Serial connection on /dev/ttyUSB0\n");
        ctx = modbus_new_rtu("/dev/ttyUSB0", 115200, 'N', 8, 1);
        modbus_set_slave(ctx, 1);
        modbus_connect(ctx);
	printf("Serial connection started!\n");
    }    
    int seed = time(NULL);
    srand(seed);
    for(;;) {
        uint8_t query[MODBUS_TCP_MAX_ADU_LENGTH];

        rc = modbus_receive(ctx, query);
        if (rc >= 0) {
	    int nToShow = 8;
	    int i=0;

	    printf("Replying to request num bytes=%i (",rc);
	    for(i=0;i<rc;i++)
	      printf("%i, ",query[i]);
	    printf(")\n");
	      
            modbus_reply(ctx, query, rc, mb_mapping);
	    
	    // after each communication, show the first ? ModBus registers so you can see what is happening
	    printf("tab_bits = ");
	    for( i=0;i<nToShow;i++)
		printf("%i, ",mb_mapping->tab_bits[i]);
	    printf("\n");
	    
	    printf("tab_input_bits = ");
	    for( i=0;i<nToShow;i++)
		printf("%i, ",mb_mapping->tab_input_bits[i]);
	    printf("\n");

	    printf("tab_input_registers = ");
	    for( i=0;i<nToShow;i++)
		printf("%i, ",mb_mapping->tab_input_registers[i]);
	    printf("\n");

	    printf("tab_registers = ");
	    for( i=0;i<nToShow;i++)
		printf("%i, ",mb_mapping->tab_registers[i]);
	    printf("\n");
	    
	    // every time we do a communication, update a bunch of the registers so we have something interesting to plot on the graphs
            //mb_mapping->tab_registers[0] = 0.529; // increment the holding reg 0 for each read
            //int random;
	    //random = 1 + (rand() % 10);// this register is a full scale random number 0 - 0xffff
   	   // mb_mapping->tab_input_registers[0] = 2.22; // version number
	    int data;
	    FILE *myFile;
	    myFile = fopen("data", "r");
	    fscanf(myFile, "%6d", &data);
	    fclose(myFile);
	    int finalData;
	    finalData = data/100;
            for( i=0;i<nToShow;i++)
	    {
		mb_mapping->tab_registers[i] = i;
		mb_mapping->tab_input_registers[i] = finalData;
	    }
        } else {
            /* Connection closed by the client or server */
            printf("Con Closed.\n");
	    modbus_close(ctx); // close
	    // immediately start waiting for another request again
            modbus_tcp_accept(ctx, &socket);
        }
    }

    printf("Quit the loop: %s\n", modbus_strerror(errno));

    modbus_mapping_free(mb_mapping);
    close(socket);
    modbus_free(ctx);

    return 0;
}

