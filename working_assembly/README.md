# ZIPORD

ZIPORD is a Windows Forms application written in C# that allows you to connect to a remote server via SSH, search for ZIP files in a specified directory, and download them via SCP.

---

## 📝 Key Features

- Connect to a remote server via SSH.
- Automatically search for ZIP files in the specified directory and subdirectories.
- Display found files with the option to download each file individually.
- Support changing the search directory without restarting the application.
- User-friendly graphical interface.

---

## 💻 Technologies and Libraries

- **C#** and **.NET Framework**
- **Windows Forms** for the interface
- **Renci.SshNet** for SSH and SCP connections
- **Org.BouncyCastle** (used for cryptography)
- **System.Runtime.InteropServices** for dragging the form with the mouse

---

## ⚙️ Installation and Running

1. Download the working assembly file.  
2. Open it and run `ZIPORD.exe`.  

---

## 🖥 Usage

1. Enter the server connection details:  
   - IP address  
   - Port (default `22`)  
   - Username and password  
   - Initial directory to search for ZIP files  
2. Click the **Connect** button.  
3. After a successful connection, the ZIP file search form will open.  
4. There is also a search bar; pressing Enter will change the search directory.  
5. Found files will appear in the list with a **Download** button for each file.  
6. Downloading will save the file to your local computer through the standard save dialog.  

---

## ⚠️ Notes

- Make sure the server user has read permissions for the specified directories.  
- Some directories may require `sudo` permissions (files will not display without them).  
- Connection and search errors are displayed via MessageBox.
