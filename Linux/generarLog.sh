#!/bin/bash

# Crear access.log y rotaciones
cat > access.log << 'EOF'
127.0.0.1 - - [10/May/2025:10:15:32 +0000] "GET /index.html HTTP/1.1" 200 1043
192.168.1.10 - - [10/May/2025:10:16:01 +0000] "POST /login HTTP/1.1" 302 512
EOF

cat > access.log.1 << 'EOF'
10.0.0.5 - - [09/May/2025:14:45:22 +0000] "GET /dashboard HTTP/1.1" 200 1587
127.0.0.1 - - [09/May/2025:14:46:00 +0000] "GET /admin HTTP/1.1" 403 720
EOF

cat > access.log.2 << 'EOF'
192.168.1.15 - - [08/May/2025:08:22:12 +0000] "GET /favicon.ico HTTP/1.1" 404 209
10.0.0.5 - - [08/May/2025:08:23:55 +0000] "GET /report.pdf HTTP/1.1" 200 2304
EOF

# Crear error.log y rotaciones
cat > error.log << 'EOF'
[Sat May 10 10:15:35.123456 2025] [core:error] [pid 1234] [client 127.0.0.1:54321] AH00126: Invalid URI in request GET /badurl HTTP/1.1
[Sat May 10 10:16:10.654321 2025] [authz_core:error] [pid 1235] [client 127.0.0.1:54322] AH01630: client denied by server configuration: /var/www/html/admin
EOF

cat > error.log.1 << 'EOF'
[Fri May 09 14:17:45.789012 2025] [php7:warn] [pid 1236] [client 192.168.1.10:54323] PHP Warning: Division by zero in /var/www/html/calc.php on line 12
[Fri May 09 14:18:02.111111 2025] [ssl:warn] [pid 1237] AH01909: certificate does NOT match server name
EOF

cat > error.log.2 << 'EOF'
[Thu May 08 08:12:01.999999 2025] [mpm_prefork:notice] [pid 1238] AH00163: Apache/2.4.29 configured -- resuming normal operations
[Thu May 08 08:12:10.123456 2025] [core:notice] [pid 1238] AH00094: Command line: '/usr/sbin/apache2'
EOF