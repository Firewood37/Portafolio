# Función para realizar la conección con la bascula y comprobar que todo este correcto
def conectar(selPuerto):
    global ser
    if selPuerto != "": # Seleccionó un elemento de la lista
        ser = serial.Serial(selPuerto, 9600, timeout=1) # Hace la conección
        if (ser.is_open): # Si la conección es exitosa...
            ser.write(b'O') # Envía un bite de prueba y lo lee
            resp = ser.read().decode('utf-8')
            rc = resp
            if (resp != ""): # Si hay una respuesta, selecciona el puerto como bascula
                while resp != "" and resp != b'\n':
                    rc += ser.read().decode('utf-8')
                messagebox.showinfo("Respuesta", f"Se seleccióno el puerto {selPuerto}\n{rc}")
                ser.close()
                puerto = selPuerto
                with open(archivoData, 'wb') as file:
                    pickle.dump(puerto, file)
    else:
        messagebox.showinfo("Respuesta", "Porfavor, selecciona un puerto")
        
r = ""
# Función para leer el peso del serial
def leerPeso():
    global valorPeso
    while leerPesoActivo:
        ser.write(b'P\n')
        resp = ser.readline().decode('utf-8').strip()
        if resp:
            if resp == b'\n':
                r = float(r.split("\t")[1].strip().split()[0])
                print(r)
                valorPeso.set(float(r))
                varValorPeso.set(f"{valorPeso.get()} kg")
                if idProd >= 0: varPrecioPeso.set(f"${valorPeso.get() * datosAMostrar[idProd][1]}")
                r = ""
            else:
                r += resp.decode('utf-8')
        sleep(0.5)

def leerProductos():
    wsProductos = wbProductos.worksheets[0]
    
    productos.clear()
    
    for row in wsProductos.iter_rows(min_col=3,min_row=4,values_only=True):
        if row[0] == None:
            break
        productos.append(list(row))
leerProductos()