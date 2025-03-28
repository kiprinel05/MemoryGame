**Plan de realizare a jocului Memory**

---

## **1. Definirea Arhitecturii și Tehnologiilor**

- Limbaj: C#
- Tehnologie UI: WPF (Windows Presentation Foundation)
- Arhitectură: MVVM (Model-View-ViewModel)
- Persistență date: JSON pentru salvare utilizatori, statistici și starea jocului
- Comenzi: ICommand pentru interacțiunea cu UI
- Data Binding pentru legătura dintre ViewModel și View
- UI modern cu design atractiv

---

## **2. Structura Aplicației**

### **2.1. Modele (Model)**

- `UserModel`: Reprezintă utilizatorul (nume, cale imagine avatar).
- `GameModel`: Reprezintă starea jocului (dimensiuni tablă, imagini asociate, timp rămas, timp trecut, cărți întoarse, cărți rămase).
- `StatisticsModel`: Stochează statistici ale utilizatorilor (număr de jocuri jucate și câștigate).

### **2.2. Vizualizări (View)**

- `LoginView.xaml`: Pagina de autentificare/creare utilizator, include upload avatar.
- `GameView.xaml`: Interfața principală a jocului.
- `StatisticsView.xaml`: Pagina cu statistici.
- `HelpView.xaml`: Fereastra About.

### **2.3. Vizualizări-Model (ViewModel)**

- `LoginViewModel`: Gestionează autentificarea, gestionarea utilizatorilor și upload avatar.
- `GameViewModel`: Gestionează logica jocului (inițializare, verificare perechi, timer, salvare joc cu toate datele relevante).
- `StatisticsViewModel`: Gestionează încărcarea și salvarea statisticilor utilizatorilor.

---

## **3. Funcționalități Implementate**

### **3.1. Login și Gestionare Utilizatori**

- Permite alegerea unui utilizator existent.
- Permite crearea unui nou utilizator și asocierea unei imagini de avatar (upload JPG, PNG, GIF).
- Buton „Delete User” pentru ștergerea utilizatorului și a datelor asociate.
- Persistența utilizatorilor într-un fișier JSON.

### **3.2. Implementarea Jocului (GameView)**

- Meniu File:
  - **Category**: Selectează categoria de imagini.
  - **New Game**: Inițializează un nou joc cu plasare aleatorie a perechilor de imagini.
  - **Open Game**: Încarcă un joc salvat (doar de către utilizatorul care l-a salvat).
  - **Save Game**: Salvează starea curentă a jocului în JSON (include timp rămas, timp trecut, cărți întoarse, cărți rămase).
  - **Statistics**: Afișează statistici ale utilizatorilor.
  - **Exit**: Revine la LoginView.
- Meniu Options:
  - **Standard**: Joc 4x4.
  - **Custom**: Joc MxN (dimensiuni selectabile între 2x2 și 6x6, cu număr par de jetoane).
- Timer afișat, contorizând timpul rămas.
- Se verifică potrivirea perechilor:
  - Dacă două jetoane sunt identice, rămân vizibile.
  - Dacă nu, se întorc cu spatele.
  - Nu permite selectarea aceluiași jeton consecutiv.
- Condiții de sfârșit: toate perechile găsite (victorie) sau expirarea timpului (pierdere).

### **3.3. Salvarea și Restaurarea Jocului**

- Salvează starea curentă a jocului într-un fișier JSON.
- Include:
  - Categoria de imagini
  - Configurația tablei
  - Timp rămas și timp scurs
  - Cărți întoarse și cărți rămase
  - Utilizatorul care a salvat jocul (doar acesta îl poate deschide)
- Încarcă jocul salvat și permite continuarea acestuia.

### **3.4. Gestionarea Statisticilor**

- Înregistrarea rezultatelor fiecărui utilizator (jocuri jucate/câștigate).
- Afișare într-un tabel cu utilizatorii și scorurile lor.

### **3.5. Funcționalități Suplimentare**

- Help -> About cu detalii despre student.
- Implementare comenzilor (ICommand) pentru interacțiunea UI.
- Utilizarea Data Binding pentru actualizarea UI în mod dinamic.
- UI modern cu stil minimalist, animații și efecte vizuale atractive.

---

## **4. Tehnologii și Biblioteci Utilizate**

- .NET 6+ / .NET Core
- WPF cu MVVM
- Newtonsoft.Json pentru serializare JSON
- ICommand și RelayCommand pentru gestionarea comenzilor în MVVM
- Binding pentru legătura UI-ViewModel
- ModernWpf sau MahApps.Metro pentru un UI modern

---

## **5. Planificare și Etape de Dezvoltare**

1. **Day 1:** Configurarea proiectului, crearea modelelor și ViewModel-urilor.
2. **Day 2:** Implementarea LoginView și gestionarea utilizatorilor (inclusiv upload avatar).
3. **Day 3:** Implementarea GameView și logica jocului, salvarea datelor în JSON.
4. **Day 4:** Salvarea/restaurarea jocului și gestionarea statisticilor.
5. **Day 5:** Optimizare, testare, UI modern, refactorizare cod și documentație.
