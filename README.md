
Philips-Intellivue Interface protocol library

There are two parts 1. Interface and data capture 2. Data decoding and storing [followed by Philips Interface Protocol]

THE INTERFACE AND DATA CAPTURE

Interface can be UART/Serial port or LAN. The LAN Interface work is not completed yet, however, the module is coded and somebofy may test its performance. Its a big tool with huge functionality. If difficult, user may use Wireshark to capture the raw data. Just grab all data (source port, destination port) from Port 24000 (med-ltp port)

DATA DECODING AND STORING The module has two primary components : Numeric data, Waveform data. In this library, the Numeric component is handled. For numeric ploting, user may need to tweek and compare with real instrument.

You may put your comments at intellivue.philips@gmail.com
