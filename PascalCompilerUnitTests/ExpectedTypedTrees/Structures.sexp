(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'S'
			Letter 't'
			Letter 'r'
			Letter 'u'
			Letter 'c'
			Letter 't'
			Letter 'u'
			Letter 'r'
			Letter 'e'
			Letter 's'
		)
		OpenParen '('
		(ProgramParameterList
			(IdentifierList
				(Identifier -> variable output: text
					Letter 'o'
					Letter 'u'
					Letter 't'
					Letter 'p'
					Letter 'u'
					Letter 't'
				)
			)
		)
		CloseParen ')'
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart
				Const 'const'
				(ConstantDefinition
					(Identifier -> constant size = 5
						Letter 's'
						Letter 'i'
						Letter 'z'
						Letter 'e'
					)
					Equals '='
					(Constant : integer
						(UnsignedNumber : integer
							(UnsignedInteger : integer
								(DigitSequence
									Digit '5'
								)
							)
						)
					)
				)
				Semicolon ';'
			)
			(TypeDefinitionPart
				Type 'type'
				(TypeDefinition
					(Identifier -> type index
						Letter 'i'
						Letter 'n'
						Letter 'd'
						Letter 'e'
						Letter 'x'
					)
					Equals '='
					(TypeDenoter : index
						(NewType : index
							(NewOrdinalType : index
								(SubrangeType : index
									(Constant : integer
										(UnsignedNumber : integer
											(UnsignedInteger : integer
												(DigitSequence
													Digit '1'
												)
											)
										)
									)
									DoubleDot '..'
									(Constant : integer
										(ConstantIdentifier : integer
											(Identifier -> constant size = 5
												Letter 's'
												Letter 'i'
												Letter 'z'
												Letter 'e'
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type vector
						Letter 'v'
						Letter 'e'
						Letter 'c'
						Letter 't'
						Letter 'o'
						Letter 'r'
					)
					Equals '='
					(TypeDenoter : vector
						(NewType : vector
							(NewStructuredType : vector
								(UnpackedStructuredType : vector
									(ArrayType : vector
										Array 'array'
										LeftSquareBracket '['
										(IndexType : index
											(OrdinalType : index
												(OrdinalTypeIdentifier : index
													(TypeIdentifier : index
														(Identifier -> type index
															Letter 'i'
															Letter 'n'
															Letter 'd'
															Letter 'e'
															Letter 'x'
														)
													)
												)
											)
										)
										RightSquareBracket ']'
										Of 'of'
										(ComponentType : real
											(TypeDenoter : real
												(TypeIdentifier : real
													(Identifier -> type real
														Letter 'r'
														Letter 'e'
														Letter 'a'
														Letter 'l'
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type matrix
						Letter 'm'
						Letter 'a'
						Letter 't'
						Letter 'r'
						Letter 'i'
						Letter 'x'
					)
					Equals '='
					(TypeDenoter : matrix
						(NewType : matrix
							(NewStructuredType : matrix
								(UnpackedStructuredType : matrix
									(ArrayType : matrix
										Array 'array'
										LeftSquareBracket '['
										(IndexType : index
											(OrdinalType : index
												(OrdinalTypeIdentifier : index
													(TypeIdentifier : index
														(Identifier -> type index
															Letter 'i'
															Letter 'n'
															Letter 'd'
															Letter 'e'
															Letter 'x'
														)
													)
												)
											)
										)
										Comma ','
										(IndexType : index
											(OrdinalType : index
												(OrdinalTypeIdentifier : index
													(TypeIdentifier : index
														(Identifier -> type index
															Letter 'i'
															Letter 'n'
															Letter 'd'
															Letter 'e'
															Letter 'x'
														)
													)
												)
											)
										)
										RightSquareBracket ']'
										Of 'of'
										(ComponentType : real
											(TypeDenoter : real
												(TypeIdentifier : real
													(Identifier -> type real
														Letter 'r'
														Letter 'e'
														Letter 'a'
														Letter 'l'
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type name
						Letter 'n'
						Letter 'a'
						Letter 'm'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : name
						(NewType : name
							(NewStructuredType : name
								Packed 'packed'
								(UnpackedStructuredType : name
									(ArrayType : name
										Array 'array'
										LeftSquareBracket '['
										(IndexType : 1..8
											(OrdinalType : 1..8
												(NewOrdinalType : 1..8
													(SubrangeType : 1..8
														(Constant : integer
															(UnsignedNumber : integer
																(UnsignedInteger : integer
																	(DigitSequence
																		Digit '1'
																	)
																)
															)
														)
														DoubleDot '..'
														(Constant : integer
															(UnsignedNumber : integer
																(UnsignedInteger : integer
																	(DigitSequence
																		Digit '8'
																	)
																)
															)
														)
													)
												)
											)
										)
										RightSquareBracket ']'
										Of 'of'
										(ComponentType : char
											(TypeDenoter : char
												(TypeIdentifier : char
													(Identifier -> type char
														Letter 'c'
														Letter 'h'
														Letter 'a'
														Letter 'r'
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type shape
						Letter 's'
						Letter 'h'
						Letter 'a'
						Letter 'p'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : shape
						(NewType : shape
							(NewOrdinalType : shape
								(EnumeratedType : shape
									OpenParen '('
									(IdentifierList
										(Identifier -> constant circle = circle
											Letter 'c'
											Letter 'i'
											Letter 'r'
											Letter 'c'
											Letter 'l'
											Letter 'e'
										)
										Comma ','
										(Identifier -> constant square = square
											Letter 's'
											Letter 'q'
											Letter 'u'
											Letter 'a'
											Letter 'r'
											Letter 'e'
										)
										Comma ','
										(Identifier -> constant triangle = triangle
											Letter 't'
											Letter 'r'
											Letter 'i'
											Letter 'a'
											Letter 'n'
											Letter 'g'
											Letter 'l'
											Letter 'e'
										)
									)
									CloseParen ')'
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type figure
						Letter 'f'
						Letter 'i'
						Letter 'g'
						Letter 'u'
						Letter 'r'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : figure
						(NewType : figure
							(NewStructuredType : figure
								(UnpackedStructuredType : figure
									(RecordType : figure
										Record 'record'
										(FieldList
											(FixedPart
												(RecordSection
													(IdentifierList
														(Identifier -> field area: real
															Letter 'a'
															Letter 'r'
															Letter 'e'
															Letter 'a'
														)
													)
													Colon ':'
													(TypeDenoter : real
														(TypeIdentifier : real
															(Identifier -> type real
																Letter 'r'
																Letter 'e'
																Letter 'a'
																Letter 'l'
															)
														)
													)
												)
											)
											Semicolon ';'
											(VariantPart
												Case 'case'
												(VariantSelector
													(TagField
														(Identifier -> field kind: shape
															Letter 'k'
															Letter 'i'
															Letter 'n'
															Letter 'd'
														)
													)
													Colon ':'
													(TagType : shape
														(OrdinalTypeIdentifier : shape
															(TypeIdentifier : shape
																(Identifier -> type shape
																	Letter 's'
																	Letter 'h'
																	Letter 'a'
																	Letter 'p'
																	Letter 'e'
																)
															)
														)
													)
												)
												Of 'of'
												(Variant
													(CaseConstantList
														(CaseConstant : shape
															(Constant : shape
																(ConstantIdentifier : shape
																	(Identifier -> constant circle = circle
																		Letter 'c'
																		Letter 'i'
																		Letter 'r'
																		Letter 'c'
																		Letter 'l'
																		Letter 'e'
																	)
																)
															)
														)
													)
													Colon ':'
													OpenParen '('
													(FieldList
														(FixedPart
															(RecordSection
																(IdentifierList
																	(Identifier -> field radius: real
																		Letter 'r'
																		Letter 'a'
																		Letter 'd'
																		Letter 'i'
																		Letter 'u'
																		Letter 's'
																	)
																)
																Colon ':'
																(TypeDenoter : real
																	(TypeIdentifier : real
																		(Identifier -> type real
																			Letter 'r'
																			Letter 'e'
																			Letter 'a'
																			Letter 'l'
																		)
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
												Semicolon ';'
												(Variant
													(CaseConstantList
														(CaseConstant : shape
															(Constant : shape
																(ConstantIdentifier : shape
																	(Identifier -> constant square = square
																		Letter 's'
																		Letter 'q'
																		Letter 'u'
																		Letter 'a'
																		Letter 'r'
																		Letter 'e'
																	)
																)
															)
														)
													)
													Colon ':'
													OpenParen '('
													(FieldList
														(FixedPart
															(RecordSection
																(IdentifierList
																	(Identifier -> field side: real
																		Letter 's'
																		Letter 'i'
																		Letter 'd'
																		Letter 'e'
																	)
																)
																Colon ':'
																(TypeDenoter : real
																	(TypeIdentifier : real
																		(Identifier -> type real
																			Letter 'r'
																			Letter 'e'
																			Letter 'a'
																			Letter 'l'
																		)
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
												Semicolon ';'
												(Variant
													(CaseConstantList
														(CaseConstant : shape
															(Constant : shape
																(ConstantIdentifier : shape
																	(Identifier -> constant triangle = triangle
																		Letter 't'
																		Letter 'r'
																		Letter 'i'
																		Letter 'a'
																		Letter 'n'
																		Letter 'g'
																		Letter 'l'
																		Letter 'e'
																	)
																)
															)
														)
													)
													Colon ':'
													OpenParen '('
													(FieldList
														(FixedPart
															(RecordSection
																(IdentifierList
																	(Identifier -> field base: real
																		Letter 'b'
																		Letter 'a'
																		Letter 's'
																		Letter 'e'
																	)
																	Comma ','
																	(Identifier -> field height: real
																		Letter 'h'
																		Letter 'e'
																		Letter 'i'
																		Letter 'g'
																		Letter 'h'
																		Letter 't'
																	)
																)
																Colon ':'
																(TypeDenoter : real
																	(TypeIdentifier : real
																		(Identifier -> type real
																			Letter 'r'
																			Letter 'e'
																			Letter 'a'
																			Letter 'l'
																		)
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
											)
										)
										End 'end'
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type charset
						Letter 'c'
						Letter 'h'
						Letter 'a'
						Letter 'r'
						Letter 's'
						Letter 'e'
						Letter 't'
					)
					Equals '='
					(TypeDenoter : charset
						(NewType : charset
							(NewStructuredType : charset
								(UnpackedStructuredType : charset
									(SetType : charset
										Set 'set'
										Of 'of'
										(BaseType : char
											(OrdinalType : char
												(OrdinalTypeIdentifier : char
													(TypeIdentifier : char
														(Identifier -> type char
															Letter 'c'
															Letter 'h'
															Letter 'a'
															Letter 'r'
														)
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type link
						Letter 'l'
						Letter 'i'
						Letter 'n'
						Letter 'k'
					)
					Equals '='
					(TypeDenoter : link
						(NewType : link
							(NewPointerType : link
								UpArrow '^'
								(DomainType
									(TypeIdentifier : node
										(Identifier -> type node
											Letter 'n'
											Letter 'o'
											Letter 'd'
											Letter 'e'
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type node
						Letter 'n'
						Letter 'o'
						Letter 'd'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : node
						(NewType : node
							(NewStructuredType : node
								(UnpackedStructuredType : node
									(RecordType : node
										Record 'record'
										(FieldList
											(FixedPart
												(RecordSection
													(IdentifierList
														(Identifier -> field value: integer
															Letter 'v'
															Letter 'a'
															Letter 'l'
															Letter 'u'
															Letter 'e'
														)
													)
													Colon ':'
													(TypeDenoter : integer
														(TypeIdentifier : integer
															(Identifier -> type integer
																Letter 'i'
																Letter 'n'
																Letter 't'
																Letter 'e'
																Letter 'g'
																Letter 'e'
																Letter 'r'
															)
														)
													)
												)
												Semicolon ';'
												(RecordSection
													(IdentifierList
														(Identifier -> field next: link
															Letter 'n'
															Letter 'e'
															Letter 'x'
															Letter 't'
														)
													)
													Colon ':'
													(TypeDenoter : link
														(TypeIdentifier : link
															(Identifier -> type link
																Letter 'l'
																Letter 'i'
																Letter 'n'
																Letter 'k'
															)
														)
													)
												)
											)
										)
										End 'end'
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type intfile
						Letter 'i'
						Letter 'n'
						Letter 't'
						Letter 'f'
						Letter 'i'
						Letter 'l'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : intfile
						(NewType : intfile
							(NewStructuredType : intfile
								(UnpackedStructuredType : intfile
									(FileType : intfile
										File 'file'
										Of 'of'
										(ComponentType : integer
											(TypeDenoter : integer
												(TypeIdentifier : integer
													(Identifier -> type integer
														Letter 'i'
														Letter 'n'
														Letter 't'
														Letter 'e'
														Letter 'g'
														Letter 'e'
														Letter 'r'
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
			)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable v: vector
							Letter 'v'
						)
					)
					Colon ':'
					(TypeDenoter : vector
						(TypeIdentifier : vector
							(Identifier -> type vector
								Letter 'v'
								Letter 'e'
								Letter 'c'
								Letter 't'
								Letter 'o'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable m: matrix
							Letter 'm'
						)
					)
					Colon ':'
					(TypeDenoter : matrix
						(TypeIdentifier : matrix
							(Identifier -> type matrix
								Letter 'm'
								Letter 'a'
								Letter 't'
								Letter 'r'
								Letter 'i'
								Letter 'x'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable nm: name
							Letter 'n'
							Letter 'm'
						)
					)
					Colon ':'
					(TypeDenoter : name
						(TypeIdentifier : name
							(Identifier -> type name
								Letter 'n'
								Letter 'a'
								Letter 'm'
								Letter 'e'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable f: figure
							Letter 'f'
						)
					)
					Colon ':'
					(TypeDenoter : figure
						(TypeIdentifier : figure
							(Identifier -> type figure
								Letter 'f'
								Letter 'i'
								Letter 'g'
								Letter 'u'
								Letter 'r'
								Letter 'e'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable letters: charset
							Letter 'l'
							Letter 'e'
							Letter 't'
							Letter 't'
							Letter 'e'
							Letter 'r'
							Letter 's'
						)
					)
					Colon ':'
					(TypeDenoter : charset
						(TypeIdentifier : charset
							(Identifier -> type charset
								Letter 'c'
								Letter 'h'
								Letter 'a'
								Letter 'r'
								Letter 's'
								Letter 'e'
								Letter 't'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable digits: set of '0'..'9'
							Letter 'd'
							Letter 'i'
							Letter 'g'
							Letter 'i'
							Letter 't'
							Letter 's'
						)
					)
					Colon ':'
					(TypeDenoter : set of '0'..'9'
						(NewType : set of '0'..'9'
							(NewStructuredType : set of '0'..'9'
								(UnpackedStructuredType : set of '0'..'9'
									(SetType : set of '0'..'9'
										Set 'set'
										Of 'of'
										(BaseType : '0'..'9'
											(OrdinalType : '0'..'9'
												(NewOrdinalType : '0'..'9'
													(SubrangeType : '0'..'9'
														(Constant : char
															CharacterString ''0''
														)
														DoubleDot '..'
														(Constant : char
															CharacterString ''9''
														)
													)
												)
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable p: link
							Letter 'p'
						)
						Comma ','
						(Identifier -> variable q: link
							Letter 'q'
						)
					)
					Colon ':'
					(TypeDenoter : link
						(TypeIdentifier : link
							(Identifier -> type link
								Letter 'l'
								Letter 'i'
								Letter 'n'
								Letter 'k'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable data: intfile
							Letter 'd'
							Letter 'a'
							Letter 't'
							Letter 'a'
						)
					)
					Colon ':'
					(TypeDenoter : intfile
						(TypeIdentifier : intfile
							(Identifier -> type intfile
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'f'
								Letter 'i'
								Letter 'l'
								Letter 'e'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable t: text
							Letter 't'
						)
					)
					Colon ':'
					(TypeDenoter : text
						(TypeIdentifier : text
							(Identifier -> type text
								Letter 't'
								Letter 'e'
								Letter 'x'
								Letter 't'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable i: index
							Letter 'i'
						)
						Comma ','
						(Identifier -> variable j: index
							Letter 'j'
						)
					)
					Colon ':'
					(TypeDenoter : index
						(TypeIdentifier : index
							(Identifier -> type index
								Letter 'i'
								Letter 'n'
								Letter 'd'
								Letter 'e'
								Letter 'x'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable total: real
							Letter 't'
							Letter 'o'
							Letter 't'
							Letter 'a'
							Letter 'l'
						)
					)
					Colon ':'
					(TypeDenoter : real
						(TypeIdentifier : real
							(Identifier -> type real
								Letter 'r'
								Letter 'e'
								Letter 'a'
								Letter 'l'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable : index
											(EntireVariable : index
												(VariableIdentifier : index
													(Identifier -> variable i: index
														Letter 'i'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue : integer
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										To 'to'
										(FinalValue : integer
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(ConstantIdentifier : integer
																	(Identifier -> constant size = 5
																		Letter 's'
																		Letter 'i'
																		Letter 'z'
																		Letter 'e'
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(StructuredStatement
												(CompoundStatement
													Begin 'begin'
													(StatementSequence
														(Statement
															(SimpleStatement
																(AssignmentStatement
																	(VariableAccess : real
																		(ComponentVariable : real
																			(IndexedVariable : real
																				(ArrayVariable : vector
																					(VariableAccess : vector
																						(EntireVariable : vector
																							(VariableIdentifier : vector
																								(Identifier -> variable v: vector
																									Letter 'v'
																								)
																							)
																						)
																					)
																				)
																				LeftSquareBracket '['
																				(IndexExpression : index
																					(Expression : index
																						(SimpleExpression : index
																							(Term : index
																								(Factor : index
																									(VariableAccess : index
																										(EntireVariable : index
																											(VariableIdentifier : index
																												(Identifier -> variable i: index
																													Letter 'i'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				RightSquareBracket ']'
																			)
																		)
																	)
																	Walrus ':='
																	(Expression : real
																		(SimpleExpression : real
																			(Term : real
																				(Factor : index
																					(VariableAccess : index
																						(EntireVariable : index
																							(VariableIdentifier : index
																								(Identifier -> variable i: index
																									Letter 'i'
																								)
																							)
																						)
																					)
																				)
																				(MultiplyingOperator
																					Asterisk '*'
																				)
																				(Factor : real
																					(UnsignedConstant : real
																						(UnsignedNumber : real
																							(UnsignedReal : real
																								(DigitSequence
																									Digit '1'
																								)
																								Dot '.'
																								(FractionalPart
																									(DigitSequence
																										Digit '5'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(StructuredStatement
																(RepetitiveStatement
																	(ForStatement
																		For 'for'
																		(ControlVariable : index
																			(EntireVariable : index
																				(VariableIdentifier : index
																					(Identifier -> variable j: index
																						Letter 'j'
																					)
																				)
																			)
																		)
																		Walrus ':='
																		(InitialValue : integer
																			(Expression : integer
																				(SimpleExpression : integer
																					(Term : integer
																						(Factor : integer
																							(UnsignedConstant : integer
																								(UnsignedNumber : integer
																									(UnsignedInteger : integer
																										(DigitSequence
																											Digit '1'
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		To 'to'
																		(FinalValue : integer
																			(Expression : integer
																				(SimpleExpression : integer
																					(Term : integer
																						(Factor : integer
																							(UnsignedConstant : integer
																								(ConstantIdentifier : integer
																									(Identifier -> constant size = 5
																										Letter 's'
																										Letter 'i'
																										Letter 'z'
																										Letter 'e'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		Do 'do'
																		(Statement
																			(SimpleStatement
																				(AssignmentStatement
																					(VariableAccess : real
																						(ComponentVariable : real
																							(IndexedVariable : real
																								(ArrayVariable : matrix
																									(VariableAccess : matrix
																										(EntireVariable : matrix
																											(VariableIdentifier : matrix
																												(Identifier -> variable m: matrix
																													Letter 'm'
																												)
																											)
																										)
																									)
																								)
																								LeftSquareBracket '['
																								(IndexExpression : index
																									(Expression : index
																										(SimpleExpression : index
																											(Term : index
																												(Factor : index
																													(VariableAccess : index
																														(EntireVariable : index
																															(VariableIdentifier : index
																																(Identifier -> variable i: index
																																	Letter 'i'
																																)
																															)
																														)
																													)
																												)
																											)
																										)
																									)
																								)
																								Comma ','
																								(IndexExpression : index
																									(Expression : index
																										(SimpleExpression : index
																											(Term : index
																												(Factor : index
																													(VariableAccess : index
																														(EntireVariable : index
																															(VariableIdentifier : index
																																(Identifier -> variable j: index
																																	Letter 'j'
																																)
																															)
																														)
																													)
																												)
																											)
																										)
																									)
																								)
																								RightSquareBracket ']'
																							)
																						)
																					)
																					Walrus ':='
																					(Expression : real
																						(SimpleExpression : real
																							(Term : real
																								(Factor : real
																									(VariableAccess : real
																										(ComponentVariable : real
																											(IndexedVariable : real
																												(ArrayVariable : vector
																													(VariableAccess : vector
																														(EntireVariable : vector
																															(VariableIdentifier : vector
																																(Identifier -> variable v: vector
																																	Letter 'v'
																																)
																															)
																														)
																													)
																												)
																												LeftSquareBracket '['
																												(IndexExpression : index
																													(Expression : index
																														(SimpleExpression : index
																															(Term : index
																																(Factor : index
																																	(VariableAccess : index
																																		(EntireVariable : index
																																			(VariableIdentifier : index
																																				(Identifier -> variable i: index
																																					Letter 'i'
																																				)
																																			)
																																		)
																																	)
																																)
																															)
																														)
																													)
																												)
																												RightSquareBracket ']'
																											)
																										)
																									)
																								)
																							)
																							(AddingOperator
																								Plus '+'
																							)
																							(Term : real
																								(Factor : real
																									(VariableAccess : real
																										(ComponentVariable : real
																											(IndexedVariable : real
																												(ArrayVariable : array[index] of real
																													(VariableAccess : array[index] of real
																														(ComponentVariable : array[index] of real
																															(IndexedVariable : array[index] of real
																																(ArrayVariable : matrix
																																	(VariableAccess : matrix
																																		(EntireVariable : matrix
																																			(VariableIdentifier : matrix
																																				(Identifier -> variable m: matrix
																																					Letter 'm'
																																				)
																																			)
																																		)
																																	)
																																)
																																LeftSquareBracket '['
																																(IndexExpression : index
																																	(Expression : index
																																		(SimpleExpression : index
																																			(Term : index
																																				(Factor : index
																																					(VariableAccess : index
																																						(EntireVariable : index
																																							(VariableIdentifier : index
																																								(Identifier -> variable j: index
																																									Letter 'j'
																																								)
																																							)
																																						)
																																					)
																																				)
																																			)
																																		)
																																	)
																																)
																																RightSquareBracket ']'
																															)
																														)
																													)
																												)
																												LeftSquareBracket '['
																												(IndexExpression : index
																													(Expression : index
																														(SimpleExpression : index
																															(Term : index
																																(Factor : index
																																	(VariableAccess : index
																																		(EntireVariable : index
																																			(VariableIdentifier : index
																																				(Identifier -> variable i: index
																																					Letter 'i'
																																				)
																																			)
																																		)
																																	)
																																)
																															)
																														)
																													)
																												)
																												RightSquareBracket ']'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													End 'end'
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : name
										(EntireVariable : name
											(VariableIdentifier : name
												(Identifier -> variable nm: name
													Letter 'n'
													Letter 'm'
												)
											)
										)
									)
									Walrus ':='
									(Expression : packed array[1..8] of char
										(SimpleExpression : packed array[1..8] of char
											(Term : packed array[1..8] of char
												(Factor : packed array[1..8] of char
													(UnsignedConstant : packed array[1..8] of char
														CharacterString ''abcdefgh''
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : char
										(ComponentVariable : char
											(IndexedVariable : char
												(ArrayVariable : name
													(VariableAccess : name
														(EntireVariable : name
															(VariableIdentifier : name
																(Identifier -> variable nm: name
																	Letter 'n'
																	Letter 'm'
																)
															)
														)
													)
												)
												LeftSquareBracket '['
												(IndexExpression : integer
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(UnsignedConstant : integer
																		(UnsignedNumber : integer
																			(UnsignedInteger : integer
																				(DigitSequence
																					Digit '1'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												RightSquareBracket ']'
											)
										)
									)
									Walrus ':='
									(Expression : char
										(SimpleExpression : char
											(Term : char
												(Factor : char
													(UnsignedConstant : char
														CharacterString ''z''
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : shape
										(ComponentVariable : shape
											(FieldDesignator : shape
												(RecordVariable : figure
													(VariableAccess : figure
														(EntireVariable : figure
															(VariableIdentifier : figure
																(Identifier -> variable f: figure
																	Letter 'f'
																)
															)
														)
													)
												)
												Dot '.'
												(FieldSpecifier : shape
													(FieldIdentifier : shape
														(Identifier -> field kind: shape
															Letter 'k'
															Letter 'i'
															Letter 'n'
															Letter 'd'
														)
													)
												)
											)
										)
									)
									Walrus ':='
									(Expression : shape
										(SimpleExpression : shape
											(Term : shape
												(Factor : shape
													(UnsignedConstant : shape
														(ConstantIdentifier : shape
															(Identifier -> constant circle = circle
																Letter 'c'
																Letter 'i'
																Letter 'r'
																Letter 'c'
																Letter 'l'
																Letter 'e'
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(ComponentVariable : real
											(FieldDesignator : real
												(RecordVariable : figure
													(VariableAccess : figure
														(EntireVariable : figure
															(VariableIdentifier : figure
																(Identifier -> variable f: figure
																	Letter 'f'
																)
															)
														)
													)
												)
												Dot '.'
												(FieldSpecifier : real
													(FieldIdentifier : real
														(Identifier -> field radius: real
															Letter 'r'
															Letter 'a'
															Letter 'd'
															Letter 'i'
															Letter 'u'
															Letter 's'
														)
													)
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : real
													(UnsignedConstant : real
														(UnsignedNumber : real
															(UnsignedReal : real
																(DigitSequence
																	Digit '2'
																)
																Dot '.'
																(FractionalPart
																	(DigitSequence
																		Digit '0'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(WithStatement
									With 'with'
									(RecordVariableList
										(RecordVariable : figure
											(VariableAccess : figure
												(EntireVariable : figure
													(VariableIdentifier : figure
														(Identifier -> variable f: figure
															Letter 'f'
														)
													)
												)
											)
										)
									)
									Do 'do'
									(Statement
										(StructuredStatement
											(CompoundStatement
												Begin 'begin'
												(StatementSequence
													(Statement
														(SimpleStatement
															(AssignmentStatement
																(VariableAccess : real
																	(ComponentVariable : real
																		(FieldDesignator : real
																			(FieldDesignatorIdentifier : real
																				(Identifier -> field area: real
																					Letter 'a'
																					Letter 'r'
																					Letter 'e'
																					Letter 'a'
																				)
																			)
																		)
																	)
																)
																Walrus ':='
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(ComponentVariable : real
																						(FieldDesignator : real
																							(FieldDesignatorIdentifier : real
																								(Identifier -> field radius: real
																									Letter 'r'
																									Letter 'a'
																									Letter 'd'
																									Letter 'i'
																									Letter 'u'
																									Letter 's'
																								)
																							)
																						)
																					)
																				)
																			)
																			(MultiplyingOperator
																				Asterisk '*'
																			)
																			(Factor : real
																				(VariableAccess : real
																					(ComponentVariable : real
																						(FieldDesignator : real
																							(FieldDesignatorIdentifier : real
																								(Identifier -> field radius: real
																									Letter 'r'
																									Letter 'a'
																									Letter 'd'
																									Letter 'i'
																									Letter 'u'
																									Letter 's'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													Semicolon ';'
													(Statement
														(SimpleStatement
															(AssignmentStatement
																(VariableAccess : shape
																	(ComponentVariable : shape
																		(FieldDesignator : shape
																			(FieldDesignatorIdentifier : shape
																				(Identifier -> field kind: shape
																					Letter 'k'
																					Letter 'i'
																					Letter 'n'
																					Letter 'd'
																				)
																			)
																		)
																	)
																)
																Walrus ':='
																(Expression : shape
																	(SimpleExpression : shape
																		(Term : shape
																			(Factor : shape
																				(UnsignedConstant : shape
																					(ConstantIdentifier : shape
																						(Identifier -> constant square = square
																							Letter 's'
																							Letter 'q'
																							Letter 'u'
																							Letter 'a'
																							Letter 'r'
																							Letter 'e'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													Semicolon ';'
													(Statement
														(SimpleStatement
															(AssignmentStatement
																(VariableAccess : real
																	(ComponentVariable : real
																		(FieldDesignator : real
																			(FieldDesignatorIdentifier : real
																				(Identifier -> field side: real
																					Letter 's'
																					Letter 'i'
																					Letter 'd'
																					Letter 'e'
																				)
																			)
																		)
																	)
																)
																Walrus ':='
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(UnsignedConstant : real
																					(UnsignedNumber : real
																						(UnsignedReal : real
																							(DigitSequence
																								Digit '3'
																							)
																							Dot '.'
																							(FractionalPart
																								(DigitSequence
																									Digit '0'
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												End 'end'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : charset
										(EntireVariable : charset
											(VariableIdentifier : charset
												(Identifier -> variable letters: charset
													Letter 'l'
													Letter 'e'
													Letter 't'
													Letter 't'
													Letter 'e'
													Letter 'r'
													Letter 's'
												)
											)
										)
									)
									Walrus ':='
									(Expression : set of char
										(SimpleExpression : set of char
											(Term : set of char
												(Factor : set of char
													(SetConstructor : set of char
														LeftSquareBracket '['
														(MemberDesignator : char
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''a''
																			)
																		)
																	)
																)
															)
															DoubleDot '..'
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''z''
																			)
																		)
																	)
																)
															)
														)
														Comma ','
														(MemberDesignator : char
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''A''
																			)
																		)
																	)
																)
															)
															DoubleDot '..'
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''Z''
																			)
																		)
																	)
																)
															)
														)
														RightSquareBracket ']'
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : set of '0'..'9'
										(EntireVariable : set of '0'..'9'
											(VariableIdentifier : set of '0'..'9'
												(Identifier -> variable digits: set of '0'..'9'
													Letter 'd'
													Letter 'i'
													Letter 'g'
													Letter 'i'
													Letter 't'
													Letter 's'
												)
											)
										)
									)
									Walrus ':='
									(Expression : set of char
										(SimpleExpression : set of char
											(Term : set of char
												(Factor : set of char
													(SetConstructor : set of char
														LeftSquareBracket '['
														(MemberDesignator : char
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''0''
																			)
																		)
																	)
																)
															)
															DoubleDot '..'
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''9''
																			)
																		)
																	)
																)
															)
														)
														RightSquareBracket ']'
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : charset
										(EntireVariable : charset
											(VariableIdentifier : charset
												(Identifier -> variable letters: charset
													Letter 'l'
													Letter 'e'
													Letter 't'
													Letter 't'
													Letter 'e'
													Letter 'r'
													Letter 's'
												)
											)
										)
									)
									Walrus ':='
									(Expression : set of char
										(SimpleExpression : set of char
											(Term : charset
												(Factor : charset
													(VariableAccess : charset
														(EntireVariable : charset
															(VariableIdentifier : charset
																(Identifier -> variable letters: charset
																	Letter 'l'
																	Letter 'e'
																	Letter 't'
																	Letter 't'
																	Letter 'e'
																	Letter 'r'
																	Letter 's'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : set of char
												(Factor : set of char
													(SetConstructor : set of char
														LeftSquareBracket '['
														(MemberDesignator : char
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''_''
																			)
																		)
																	)
																)
															)
														)
														RightSquareBracket ']'
													)
												)
											)
											(AddingOperator
												Minus '-'
											)
											(Term : set of char
												(Factor : set of char
													(SetConstructor : set of char
														LeftSquareBracket '['
														(MemberDesignator : char
															(Expression : char
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''x''
																			)
																		)
																	)
																)
															)
														)
														RightSquareBracket ']'
													)
												)
												(MultiplyingOperator
													Asterisk '*'
												)
												(Factor : charset
													(VariableAccess : charset
														(EntireVariable : charset
															(VariableIdentifier : charset
																(Identifier -> variable letters: charset
																	Letter 'l'
																	Letter 'e'
																	Letter 't'
																	Letter 't'
																	Letter 'e'
																	Letter 'r'
																	Letter 's'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(EntireVariable : real
											(VariableIdentifier : real
												(Identifier -> variable total: real
													Letter 't'
													Letter 'o'
													Letter 't'
													Letter 'a'
													Letter 'l'
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : real
													(UnsignedConstant : real
														(UnsignedNumber : real
															(UnsignedReal : real
																(DigitSequence
																	Digit '0'
																)
																Dot '.'
																(FractionalPart
																	(DigitSequence
																		Digit '0'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(IfStatement
										If 'if'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : Boolean
													(Term : Boolean
														(Factor : Boolean
															OpenParen '('
															(Expression : Boolean
																(SimpleExpression : char
																	(Term : char
																		(Factor : char
																			(UnsignedConstant : char
																				CharacterString ''q''
																			)
																		)
																	)
																)
																(RelationalOperator
																	In 'in'
																)
																(SimpleExpression : charset
																	(Term : charset
																		(Factor : charset
																			(VariableAccess : charset
																				(EntireVariable : charset
																					(VariableIdentifier : charset
																						(Identifier -> variable letters: charset
																							Letter 'l'
																							Letter 'e'
																							Letter 't'
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 's'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
														(MultiplyingOperator
															And 'and'
														)
														(Factor : Boolean
															Not 'not'
															(Factor : Boolean
																OpenParen '('
																(Expression : Boolean
																	(SimpleExpression : charset
																		(Term : charset
																			(Factor : charset
																				(VariableAccess : charset
																					(EntireVariable : charset
																						(VariableIdentifier : charset
																							(Identifier -> variable letters: charset
																								Letter 'l'
																								Letter 'e'
																								Letter 't'
																								Letter 't'
																								Letter 'e'
																								Letter 'r'
																								Letter 's'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(RelationalOperator
																		LessThanOrEqual '<='
																	)
																	(SimpleExpression : set of '0'..'9'
																		(Term : set of '0'..'9'
																			(Factor : set of '0'..'9'
																				(VariableAccess : set of '0'..'9'
																					(EntireVariable : set of '0'..'9'
																						(VariableIdentifier : set of '0'..'9'
																							(Identifier -> variable digits: set of '0'..'9'
																								Letter 'd'
																								Letter 'i'
																								Letter 'g'
																								Letter 'i'
																								Letter 't'
																								Letter 's'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
																CloseParen ')'
															)
														)
														(MultiplyingOperator
															And 'and'
														)
														(Factor : Boolean
															OpenParen '('
															(Expression : Boolean
																(SimpleExpression : charset
																	(Term : charset
																		(Factor : charset
																			(VariableAccess : charset
																				(EntireVariable : charset
																					(VariableIdentifier : charset
																						(Identifier -> variable letters: charset
																							Letter 'l'
																							Letter 'e'
																							Letter 't'
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 's'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	KetPair '<>'
																)
																(SimpleExpression : []
																	(Term : []
																		(Factor : []
																			(SetConstructor : []
																				LeftSquareBracket '['
																				RightSquareBracket ']'
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
										)
										Then 'then'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : real
														(EntireVariable : real
															(VariableIdentifier : real
																(Identifier -> variable total: real
																	Letter 't'
																	Letter 'o'
																	Letter 't'
																	Letter 'a'
																	Letter 'l'
																)
															)
														)
													)
													Walrus ':='
													(Expression : real
														(SimpleExpression : real
															(Term : real
																(Factor : real
																	(UnsignedConstant : real
																		(UnsignedNumber : real
																			(UnsignedReal : real
																				(DigitSequence
																					Digit '1'
																				)
																				Dot '.'
																				(FractionalPart
																					(DigitSequence
																						Digit '0'
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure new
											Letter 'n'
											Letter 'e'
											Letter 'w'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : link
												(EntireVariable : link
													(VariableIdentifier : link
														(Identifier -> variable p: link
															Letter 'p'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(ComponentVariable : integer
											(FieldDesignator : integer
												(RecordVariable : node
													(VariableAccess : node
														(IdentifiedVariable : node
															(PointerVariable : link
																(VariableAccess : link
																	(EntireVariable : link
																		(VariableIdentifier : link
																			(Identifier -> variable p: link
																				Letter 'p'
																			)
																		)
																	)
																)
															)
															UpArrow '^'
														)
													)
												)
												Dot '.'
												(FieldSpecifier : integer
													(FieldIdentifier : integer
														(Identifier -> field value: integer
															Letter 'v'
															Letter 'a'
															Letter 'l'
															Letter 'u'
															Letter 'e'
														)
													)
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '1'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : link
										(ComponentVariable : link
											(FieldDesignator : link
												(RecordVariable : node
													(VariableAccess : node
														(IdentifiedVariable : node
															(PointerVariable : link
																(VariableAccess : link
																	(EntireVariable : link
																		(VariableIdentifier : link
																			(Identifier -> variable p: link
																				Letter 'p'
																			)
																		)
																	)
																)
															)
															UpArrow '^'
														)
													)
												)
												Dot '.'
												(FieldSpecifier : link
													(FieldIdentifier : link
														(Identifier -> field next: link
															Letter 'n'
															Letter 'e'
															Letter 'x'
															Letter 't'
														)
													)
												)
											)
										)
									)
									Walrus ':='
									(Expression : nil
										(SimpleExpression : nil
											(Term : nil
												(Factor : nil
													(UnsignedConstant : nil
														Nil 'nil'
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure new
											Letter 'n'
											Letter 'e'
											Letter 'w'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : link
												(EntireVariable : link
													(VariableIdentifier : link
														(Identifier -> variable q: link
															Letter 'q'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : node
										(IdentifiedVariable : node
											(PointerVariable : link
												(VariableAccess : link
													(EntireVariable : link
														(VariableIdentifier : link
															(Identifier -> variable q: link
																Letter 'q'
															)
														)
													)
												)
											)
											UpArrow '^'
										)
									)
									Walrus ':='
									(Expression : node
										(SimpleExpression : node
											(Term : node
												(Factor : node
													(VariableAccess : node
														(IdentifiedVariable : node
															(PointerVariable : link
																(VariableAccess : link
																	(EntireVariable : link
																		(VariableIdentifier : link
																			(Identifier -> variable p: link
																				Letter 'p'
																			)
																		)
																	)
																)
															)
															UpArrow '^'
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(IfStatement
										If 'if'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : Boolean
													(Term : Boolean
														(Factor : Boolean
															OpenParen '('
															(Expression : Boolean
																(SimpleExpression : link
																	(Term : link
																		(Factor : link
																			(VariableAccess : link
																				(EntireVariable : link
																					(VariableIdentifier : link
																						(Identifier -> variable p: link
																							Letter 'p'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	KetPair '<>'
																)
																(SimpleExpression : nil
																	(Term : nil
																		(Factor : nil
																			(UnsignedConstant : nil
																				Nil 'nil'
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
														(MultiplyingOperator
															And 'and'
														)
														(Factor : Boolean
															OpenParen '('
															(Expression : Boolean
																(SimpleExpression : link
																	(Term : link
																		(Factor : link
																			(VariableAccess : link
																				(EntireVariable : link
																					(VariableIdentifier : link
																						(Identifier -> variable p: link
																							Letter 'p'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(RelationalOperator
																	KetPair '<>'
																)
																(SimpleExpression : link
																	(Term : link
																		(Factor : link
																			(VariableAccess : link
																				(EntireVariable : link
																					(VariableIdentifier : link
																						(Identifier -> variable q: link
																							Letter 'q'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
										)
										Then 'then'
										(Statement
											(SimpleStatement
												(ProcedureStatement
													(ProcedureIdentifier
														(Identifier -> procedure dispose
															Letter 'd'
															Letter 'i'
															Letter 's'
															Letter 'p'
															Letter 'o'
															Letter 's'
															Letter 'e'
														)
													)
													(ActualParameterList
														OpenParen '('
														(ActualParameter
															(Expression : link
																(SimpleExpression : link
																	(Term : link
																		(Factor : link
																			(VariableAccess : link
																				(EntireVariable : link
																					(VariableIdentifier : link
																						(Identifier -> variable q: link
																							Letter 'q'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														CloseParen ')'
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure dispose
											Letter 'd'
											Letter 'i'
											Letter 's'
											Letter 'p'
											Letter 'o'
											Letter 's'
											Letter 'e'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(Expression : link
												(SimpleExpression : link
													(Term : link
														(Factor : link
															(VariableAccess : link
																(EntireVariable : link
																	(VariableIdentifier : link
																		(Identifier -> variable p: link
																			Letter 'p'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure rewrite
											Letter 'r'
											Letter 'e'
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : intfile
												(EntireVariable : intfile
													(VariableIdentifier : intfile
														(Identifier -> variable data: intfile
															Letter 'd'
															Letter 'a'
															Letter 't'
															Letter 'a'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(BufferVariable : integer
											(FileVariable : intfile
												(VariableAccess : intfile
													(EntireVariable : intfile
														(VariableIdentifier : intfile
															(Identifier -> variable data: intfile
																Letter 'd'
																Letter 'a'
																Letter 't'
																Letter 'a'
															)
														)
													)
												)
											)
											UpArrow '^'
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '4'
																	Digit '2'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure put
											Letter 'p'
											Letter 'u'
											Letter 't'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : intfile
												(EntireVariable : intfile
													(VariableIdentifier : intfile
														(Identifier -> variable data: intfile
															Letter 'd'
															Letter 'a'
															Letter 't'
															Letter 'a'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure reset
											Letter 'r'
											Letter 'e'
											Letter 's'
											Letter 'e'
											Letter 't'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : intfile
												(EntireVariable : intfile
													(VariableIdentifier : intfile
														(Identifier -> variable data: intfile
															Letter 'd'
															Letter 'a'
															Letter 't'
															Letter 'a'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(WhileStatement
										While 'while'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : Boolean
													(Term : Boolean
														(Factor : Boolean
															Not 'not'
															(Factor : Boolean
																(FunctionDesignator : Boolean
																	(FunctionIdentifier
																		(Identifier -> function eof: Boolean
																			Letter 'e'
																			Letter 'o'
																			Letter 'f'
																		)
																	)
																	(ActualParameterList
																		OpenParen '('
																		(ActualParameter
																			(VariableAccess : intfile
																				(EntireVariable : intfile
																					(VariableIdentifier : intfile
																						(Identifier -> variable data: intfile
																							Letter 'd'
																							Letter 'a'
																							Letter 't'
																							Letter 'a'
																						)
																					)
																				)
																			)
																		)
																		CloseParen ')'
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(StructuredStatement
												(CompoundStatement
													Begin 'begin'
													(StatementSequence
														(Statement
															(SimpleStatement
																(AssignmentStatement
																	(VariableAccess : real
																		(EntireVariable : real
																			(VariableIdentifier : real
																				(Identifier -> variable total: real
																					Letter 't'
																					Letter 'o'
																					Letter 't'
																					Letter 'a'
																					Letter 'l'
																				)
																			)
																		)
																	)
																	Walrus ':='
																	(Expression : real
																		(SimpleExpression : real
																			(Term : real
																				(Factor : real
																					(VariableAccess : real
																						(EntireVariable : real
																							(VariableIdentifier : real
																								(Identifier -> variable total: real
																									Letter 't'
																									Letter 'o'
																									Letter 't'
																									Letter 'a'
																									Letter 'l'
																								)
																							)
																						)
																					)
																				)
																			)
																			(AddingOperator
																				Plus '+'
																			)
																			(Term : integer
																				(Factor : integer
																					(VariableAccess : integer
																						(BufferVariable : integer
																							(FileVariable : intfile
																								(VariableAccess : intfile
																									(EntireVariable : intfile
																										(VariableIdentifier : intfile
																											(Identifier -> variable data: intfile
																												Letter 'd'
																												Letter 'a'
																												Letter 't'
																												Letter 'a'
																											)
																										)
																									)
																								)
																							)
																							UpArrow '^'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(SimpleStatement
																(ProcedureStatement
																	(ProcedureIdentifier
																		(Identifier -> procedure get
																			Letter 'g'
																			Letter 'e'
																			Letter 't'
																		)
																	)
																	(ActualParameterList
																		OpenParen '('
																		(ActualParameter
																			(VariableAccess : intfile
																				(EntireVariable : intfile
																					(VariableIdentifier : intfile
																						(Identifier -> variable data: intfile
																							Letter 'd'
																							Letter 'a'
																							Letter 't'
																							Letter 'a'
																						)
																					)
																				)
																			)
																		)
																		CloseParen ')'
																	)
																)
															)
														)
													)
													End 'end'
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure rewrite
											Letter 'r'
											Letter 'e'
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : text
												(EntireVariable : text
													(VariableIdentifier : text
														(Identifier -> variable t: text
															Letter 't'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure writeln
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
											Letter 'l'
											Letter 'n'
										)
									)
									(WritelnParameterList
										OpenParen '('
										(FileVariable : text
											(VariableAccess : text
												(EntireVariable : text
													(VariableIdentifier : text
														(Identifier -> variable t: text
															Letter 't'
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : packed array[1..7] of char
												(SimpleExpression : packed array[1..7] of char
													(Term : packed array[1..7] of char
														(Factor : packed array[1..7] of char
															(UnsignedConstant : packed array[1..7] of char
																CharacterString ''total: ''
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : real
												(SimpleExpression : real
													(Term : real
														(Factor : real
															(VariableAccess : real
																(EntireVariable : real
																	(VariableIdentifier : real
																		(Identifier -> variable total: real
																			Letter 't'
																			Letter 'o'
																			Letter 't'
																			Letter 'a'
																			Letter 'l'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '6'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure reset
											Letter 'r'
											Letter 'e'
											Letter 's'
											Letter 'e'
											Letter 't'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : text
												(EntireVariable : text
													(VariableIdentifier : text
														(Identifier -> variable t: text
															Letter 't'
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(IfStatement
										If 'if'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : Boolean
													(Term : Boolean
														(Factor : Boolean
															Not 'not'
															(Factor : Boolean
																(FunctionDesignator : Boolean
																	(FunctionIdentifier
																		(Identifier -> function eoln: Boolean
																			Letter 'e'
																			Letter 'o'
																			Letter 'l'
																			Letter 'n'
																		)
																	)
																	(ActualParameterList
																		OpenParen '('
																		(ActualParameter
																			(VariableAccess : text
																				(EntireVariable : text
																					(VariableIdentifier : text
																						(Identifier -> variable t: text
																							Letter 't'
																						)
																					)
																				)
																			)
																		)
																		CloseParen ')'
																	)
																)
															)
														)
													)
												)
											)
										)
										Then 'then'
										(Statement
											(SimpleStatement
												(ProcedureStatement
													(ProcedureIdentifier
														(Identifier -> procedure readln
															Letter 'r'
															Letter 'e'
															Letter 'a'
															Letter 'd'
															Letter 'l'
															Letter 'n'
														)
													)
													(ReadlnParameterList
														OpenParen '('
														(FileVariable : text
															(VariableAccess : text
																(EntireVariable : text
																	(VariableIdentifier : text
																		(Identifier -> variable t: text
																			Letter 't'
																		)
																	)
																)
															)
														)
														CloseParen ')'
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure writeln
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
											Letter 'l'
											Letter 'n'
										)
									)
									(WritelnParameterList
										OpenParen '('
										(WriteParameter
											(Expression : name
												(SimpleExpression : name
													(Term : name
														(Factor : name
															(VariableAccess : name
																(EntireVariable : name
																	(VariableIdentifier : name
																		(Identifier -> variable nm: name
																			Letter 'n'
																			Letter 'm'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : char
												(SimpleExpression : char
													(Term : char
														(Factor : char
															(UnsignedConstant : char
																CharacterString '' ''
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : real
												(SimpleExpression : real
													(Term : real
														(Factor : real
															(VariableAccess : real
																(ComponentVariable : real
																	(FieldDesignator : real
																		(RecordVariable : figure
																			(VariableAccess : figure
																				(EntireVariable : figure
																					(VariableIdentifier : figure
																						(Identifier -> variable f: figure
																							Letter 'f'
																						)
																					)
																				)
																			)
																		)
																		Dot '.'
																		(FieldSpecifier : real
																			(FieldIdentifier : real
																				(Identifier -> field area: real
																					Letter 'a'
																					Letter 'r'
																					Letter 'e'
																					Letter 'a'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '4'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)