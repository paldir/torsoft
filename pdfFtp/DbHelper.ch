//////////////////////////////////////////////////////////////////////
///
/// <summary>
/// </summary>
///
///
/// <remarks>
/// </remarks>
///
///
/// <copyright>
/// Your-Company. All Rights Reserved.
/// </copyright>
///
//////////////////////////////////////////////////////////////////////
CLASS DbHelper
   EXPORTED:
      METHOD init
      METHOD CloseConnection
      METHOD ExecuteQuery, ExecuteStatement
      METHOD SQLSelect, SQLInsert, SQLUpdate, SQLDelete
      METHOD GetLastMessage
      CLASS METHOD CzytajZIni
   HIDDEN:
      VAR oSession
ENDCLASS

METHOD DbHelper: init(server, db)
   IF !DbeLoad("PGDBE")
      Alert("Unable to load PostgreSQL PGDBE", {"Ok"})
   ENDIF

   DbeSetDefault("PGDBE")

   cConnect:="DBE=pgdbe; SERVER="+server+"; DB="+db+"; UID=postgres; PWD=postgres"
   ::oSession:=DacSession(): New(cConnect)
RETURN self

METHOD DbHelper: CloseConnection()
   ::oSession: disconnect()
RETURN self

METHOD DbHelper: ExecuteQuery(query)
RETURN ::oSession: ExecuteQuery(query)

METHOD DbHelper: ExecuteStatement(statement)
RETURN ::oSession: ExecuteStatement(statement)

METHOD DbHelper: SQLSelect(columns, table, whereStatement, orderBy)
   query:="SELECT "

   FOR i:=1 to Len(columns)
      /*query+="CASE pg_typeof("+columns[i]+") WHEN 'character'::regtype THEN ReplacePolishSymbols("+columns[i]+")"

      ::ExecuteQuery("SELECT character_maximum_length FROM information_schema.columns WHERE table_name='"+table+"' AND column_name='"+columns[i]+"'")

      IF FCount()>0 .AND. FieldGet(1)>0
         query+="::CHARACTER("+Var2Char(FieldGet(1))+")"
      ENDIF

      query+=" ELSE "+columns[i]+" END  AS t"  */

      query+=columns[i]

      IF i<Len(columns)
         query+=","
      ENDIF

      query+=" "
   NEXT

   query+=" FROM "+table

   IF whereStatement!=NIL
      query+=" WHERE "+whereStatement
   ENDIF

   IF orderBy!=NIL
      query+=" ORDER BY "+orderBy
   ENDIF

   ::ExecuteQuery(query)
RETURN query

METHOD DbHelper: SQLInsert(table, columns, values)
   statement:="INSERT INTO "+table+" ("

   FOR i:=1 to Len(columns)-1
      statement+=columns[i]+", "
   NEXT

   statement+=columns[Len(columns)]+") VALUES ("

   FOR i:=1 to Len(values)-1
      statement+="'"+values[i]+"', "
   NEXT

   statement+="'"+values[Len(values)]+"')"
RETURN ::ExecuteStatement(statement)

METHOD DbHelper: SQLUpdate(table, columns, values, whereStatement)
   statement:="UPDATE "+table+" SET "

   FOR i:=1 to Len(columns)-1
      statement+=columns[i]+"='"+values[i]+"', "
   NEXT

   statement+=columns[Len(columns)]+"='"+values[Len(columns)]+"'"
   statement+=" WHERE "+ whereStatement
RETURN ::ExecuteStatement(statement)

METHOD DbHelper: SQLDelete(table, where)
RETURN ::ExecuteStatement("DELETE FROM "+table+" WHERE "+where)

METHOD DbHelper: GetLastMessage()
RETURN ::oSession: getLastMessage()

CLASS METHOD DbHelper: CzytajZIni(cNazwaPliku, cNazwaSekcji, cKlucz)
   #define nDlugoscLinii 1000
   LOCAL i, j, cPlik, cLinia, nIloscLinii
   LOCAL nIndeksRownosci, cAktualnyKlucz, cWartosc

   cPlik=MemoRead(cNazwaPliku)
   nIloscLinii=MlCount(cPlik)

   FOR i=1 to nIloscLinii
      cLinia=Alltrim(MemoLine(cPlik, nDlugoscLinii, i))

      IF cLinia[1]=='['
         cLinia=SubStr(cLinia, 2)
         cLinia=SubStr(cLinia, 1, Len(cLinia)-1)
         cLinia=AllTrim(cLinia)

         IF cLinia==AllTrim(cNazwaSekcji)
            FOR j=i+1 to nIloscLinii
               cLinia=Alltrim(MemoLine(cPlik, nDlugoscLinii, j))
               nIndeksRownosci=At("=", cLinia)

               IF nIndeksRownosci>0
                  cAktualnyKlucz=Alltrim(SubStr(cLinia, 1, nIndeksRownosci-1))

                  IF cAktualnyKlucz==cKlucz
                     cWartosc=Alltrim(SubStr(cLinia, nIndeksRownosci+1))

                     RETURN cWartosc
                  ENDIF
               ENDIF
            NEXT

            RETURN NIL
         ENDIF
      ENDIF
   NEXT
RETURN NIL