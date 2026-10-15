(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'R'
			Letter 'o'
			Letter 'u'
			Letter 't'
			Letter 'i'
			Letter 'n'
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
			(ConstantDefinitionPart)
			(TypeDefinitionPart
				Type 'type'
				(TypeDefinition
					(Identifier -> type row
						Letter 'r'
						Letter 'o'
						Letter 'w'
					)
					Equals '='
					(TypeDenoter : row
						(NewType : row
							(NewStructuredType : row
								(UnpackedStructuredType : row
									(ArrayType : row
										Array 'array'
										LeftSquareBracket '['
										(IndexType : 1..3
											(OrdinalType : 1..3
												(NewOrdinalType : 1..3
													(SubrangeType : 1..3
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
																		Digit '3'
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
						(Identifier -> variable count: integer
							Letter 'c'
							Letter 'o'
							Letter 'u'
							Letter 'n'
							Letter 't'
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
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable values: row
							Letter 'v'
							Letter 'a'
							Letter 'l'
							Letter 'u'
							Letter 'e'
							Letter 's'
						)
					)
					Colon ':'
					(TypeDenoter : row
						(TypeIdentifier : row
							(Identifier -> type row
								Letter 'r'
								Letter 'o'
								Letter 'w'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable r: real
							Letter 'r'
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
			(ProcedureAndFunctionDeclarationPart
				(ProcedureDeclaration
					(ProcedureHeading
						Procedure 'procedure'
						(Identifier -> procedure later
							Letter 'l'
							Letter 'a'
							Letter 't'
							Letter 'e'
							Letter 'r'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(ValueParameterSpecification
									(IdentifierList
										(Identifier -> parameter n: integer
											Letter 'n'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
					)
					Semicolon ';'
					(Directive
						Letter 'f'
						Letter 'o'
						Letter 'r'
						Letter 'w'
						Letter 'a'
						Letter 'r'
						Letter 'd'
					)
				)
				Semicolon ';'
				(FunctionDeclaration
					(FunctionHeading
						Function 'function'
						(Identifier -> function square: integer
							Letter 's'
							Letter 'q'
							Letter 'u'
							Letter 'a'
							Letter 'r'
							Letter 'e'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(ValueParameterSpecification
									(IdentifierList
										(Identifier -> parameter x: integer
											Letter 'x'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
						Colon ':'
						(ResultType : integer
							(SimpleTypeIdentifier : integer
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
					Semicolon ';'
					(FunctionBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(FunctionIdentifier
														(Identifier -> function square: integer
															Letter 's'
															Letter 'q'
															Letter 'u'
															Letter 'a'
															Letter 'r'
															Letter 'e'
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> parameter x: integer
																					Letter 'x'
																				)
																			)
																		)
																	)
																)
																(MultiplyingOperator
																	Asterisk '*'
																)
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> parameter x: integer
																					Letter 'x'
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
				Semicolon ';'
				(ProcedureDeclaration
					(ProcedureHeading
						Procedure 'procedure'
						(Identifier -> procedure swap
							Letter 's'
							Letter 'w'
							Letter 'a'
							Letter 'p'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(VariableParameterSpecification
									Var 'var'
									(IdentifierList
										(Identifier -> parameter a: integer
											Letter 'a'
										)
										Comma ','
										(Identifier -> parameter b: integer
											Letter 'b'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
					)
					Semicolon ';'
					(ProcedureBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart
								Var 'var'
								(VariableDeclaration
									(IdentifierList
										(Identifier -> variable temp: integer
											Letter 't'
											Letter 'e'
											Letter 'm'
											Letter 'p'
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
							)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable temp: integer
																	Letter 't'
																	Letter 'e'
																	Letter 'm'
																	Letter 'p'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> parameter a: integer
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
										)
										Semicolon ';'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> parameter a: integer
																	Letter 'a'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> parameter b: integer
																					Letter 'b'
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
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> parameter b: integer
																	Letter 'b'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable temp: integer
																					Letter 't'
																					Letter 'e'
																					Letter 'm'
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
											)
										)
									)
									End 'end'
								)
							)
						)
					)
				)
				Semicolon ';'
				(FunctionDeclaration
					(FunctionHeading
						Function 'function'
						(Identifier -> function factorial: integer
							Letter 'f'
							Letter 'a'
							Letter 'c'
							Letter 't'
							Letter 'o'
							Letter 'r'
							Letter 'i'
							Letter 'a'
							Letter 'l'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(ValueParameterSpecification
									(IdentifierList
										(Identifier -> parameter n: integer
											Letter 'n'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
						Colon ':'
						(ResultType : integer
							(SimpleTypeIdentifier : integer
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
					Semicolon ';'
					(FunctionBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(StructuredStatement
												(ConditionalStatement
													(IfStatement
														If 'if'
														(BooleanExpression : Boolean
															(Expression : Boolean
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(VariableAccess : integer
																				(EntireVariable : integer
																					(VariableIdentifier : integer
																						(Identifier -> parameter n: integer
																							Letter 'n'
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
														Then 'then'
														(Statement
															(SimpleStatement
																(AssignmentStatement
																	(FunctionIdentifier
																		(Identifier -> function factorial: integer
																			Letter 'f'
																			Letter 'a'
																			Letter 'c'
																			Letter 't'
																			Letter 'o'
																			Letter 'r'
																			Letter 'i'
																			Letter 'a'
																			Letter 'l'
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
														(ElsePart
															Else 'else'
															(Statement
																(SimpleStatement
																	(AssignmentStatement
																		(FunctionIdentifier
																			(Identifier -> function factorial: integer
																				Letter 'f'
																				Letter 'a'
																				Letter 'c'
																				Letter 't'
																				Letter 'o'
																				Letter 'r'
																				Letter 'i'
																				Letter 'a'
																				Letter 'l'
																			)
																		)
																		Walrus ':='
																		(Expression : integer
																			(SimpleExpression : integer
																				(Term : integer
																					(Factor : integer
																						(VariableAccess : integer
																							(EntireVariable : integer
																								(VariableIdentifier : integer
																									(Identifier -> parameter n: integer
																										Letter 'n'
																									)
																								)
																							)
																						)
																					)
																					(MultiplyingOperator
																						Asterisk '*'
																					)
																					(Factor : integer
																						(FunctionDesignator : integer
																							(FunctionIdentifier
																								(Identifier -> function factorial: integer
																									Letter 'f'
																									Letter 'a'
																									Letter 'c'
																									Letter 't'
																									Letter 'o'
																									Letter 'r'
																									Letter 'i'
																									Letter 'a'
																									Letter 'l'
																								)
																							)
																							(ActualParameterList
																								OpenParen '('
																								(ActualParameter
																									(Expression : integer
																										(SimpleExpression : integer
																											(Term : integer
																												(Factor : integer
																													(VariableAccess : integer
																														(EntireVariable : integer
																															(VariableIdentifier : integer
																																(Identifier -> parameter n: integer
																																	Letter 'n'
																																)
																															)
																														)
																													)
																												)
																											)
																											(AddingOperator
																												Minus '-'
																											)
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
				Semicolon ';'
				(ProcedureDeclaration
					(ProcedureHeading
						Procedure 'procedure'
						(Identifier -> procedure apply
							Letter 'a'
							Letter 'p'
							Letter 'p'
							Letter 'l'
							Letter 'y'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(FunctionalParameterSpecification
									(FunctionHeading
										Function 'function'
										(Identifier -> function f: integer
											Letter 'f'
										)
										(FormalParameterList
											OpenParen '('
											(FormalParameterSection
												(ValueParameterSpecification
													(IdentifierList
														(Identifier -> parameter x: integer
															Letter 'x'
														)
													)
													Colon ':'
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
											CloseParen ')'
										)
										Colon ':'
										(ResultType : integer
											(SimpleTypeIdentifier : integer
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
							Semicolon ';'
							(FormalParameterSection
								(ProceduralParameterSpecification
									(ProcedureHeading
										Procedure 'procedure'
										(Identifier -> procedure report
											Letter 'r'
											Letter 'e'
											Letter 'p'
											Letter 'o'
											Letter 'r'
											Letter 't'
										)
										(FormalParameterList
											OpenParen '('
											(FormalParameterSection
												(ValueParameterSpecification
													(IdentifierList
														(Identifier -> parameter n: integer
															Letter 'n'
														)
													)
													Colon ':'
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
											CloseParen ')'
										)
									)
								)
							)
							Semicolon ';'
							(FormalParameterSection
								(VariableParameterSpecification
									Var 'var'
									(IdentifierList
										(Identifier -> parameter target: integer
											Letter 't'
											Letter 'a'
											Letter 'r'
											Letter 'g'
											Letter 'e'
											Letter 't'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
					)
					Semicolon ';'
					(ProcedureBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> parameter target: integer
																	Letter 't'
																	Letter 'a'
																	Letter 'r'
																	Letter 'g'
																	Letter 'e'
																	Letter 't'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(FunctionDesignator : integer
																		(FunctionIdentifier
																			(Identifier -> function f: integer
																				Letter 'f'
																			)
																		)
																		(ActualParameterList
																			OpenParen '('
																			(ActualParameter
																				(Expression : integer
																					(SimpleExpression : integer
																						(Term : integer
																							(Factor : integer
																								(VariableAccess : integer
																									(EntireVariable : integer
																										(VariableIdentifier : integer
																											(Identifier -> parameter target: integer
																												Letter 't'
																												Letter 'a'
																												Letter 'r'
																												Letter 'g'
																												Letter 'e'
																												Letter 't'
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
										)
										Semicolon ';'
										(Statement
											(SimpleStatement
												(ProcedureStatement
													(ProcedureIdentifier
														(Identifier -> procedure report
															Letter 'r'
															Letter 'e'
															Letter 'p'
															Letter 'o'
															Letter 'r'
															Letter 't'
														)
													)
													(ActualParameterList
														OpenParen '('
														(ActualParameter
															(Expression : integer
																(SimpleExpression : integer
																	(Term : integer
																		(Factor : integer
																			(VariableAccess : integer
																				(EntireVariable : integer
																					(VariableIdentifier : integer
																						(Identifier -> parameter target: integer
																							Letter 't'
																							Letter 'a'
																							Letter 'r'
																							Letter 'g'
																							Letter 'e'
																							Letter 't'
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
				)
				Semicolon ';'
				(ProcedureDeclaration
					(ProcedureIdentification
						Procedure 'procedure'
						(ProcedureIdentifier
							(Identifier -> procedure later
								Letter 'l'
								Letter 'a'
								Letter 't'
								Letter 'e'
								Letter 'r'
							)
						)
					)
					Semicolon ';'
					(ProcedureBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable count: integer
																	Letter 'c'
																	Letter 'o'
																	Letter 'u'
																	Letter 'n'
																	Letter 't'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable count: integer
																					Letter 'c'
																					Letter 'o'
																					Letter 'u'
																					Letter 'n'
																					Letter 't'
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
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> parameter n: integer
																					Letter 'n'
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
				Semicolon ';'
				(FunctionDeclaration
					(FunctionHeading
						Function 'function'
						(Identifier -> function sum: integer
							Letter 's'
							Letter 'u'
							Letter 'm'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(ConformantArrayParameterSpecification
									(VariableConformantArraySpecification
										Var 'var'
										(IdentifierList
											(Identifier -> parameter a: array[integer] of integer
												Letter 'a'
											)
										)
										Colon ':'
										(ConformantArraySchema : array[integer] of integer
											(UnpackedConformantArraySchema : array[integer] of integer
												Array 'array'
												LeftSquareBracket '['
												(IndexTypeSpecification
													(Identifier -> low bound low: integer
														Letter 'l'
														Letter 'o'
														Letter 'w'
													)
													DoubleDot '..'
													(Identifier -> high bound high: integer
														Letter 'h'
														Letter 'i'
														Letter 'g'
														Letter 'h'
													)
													Colon ':'
													(OrdinalTypeIdentifier : integer
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
												RightSquareBracket ']'
												Of 'of'
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
							CloseParen ')'
						)
						Colon ':'
						(ResultType : integer
							(SimpleTypeIdentifier : integer
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
					Semicolon ';'
					(FunctionBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart
								Var 'var'
								(VariableDeclaration
									(IdentifierList
										(Identifier -> variable i: integer
											Letter 'i'
										)
										Comma ','
										(Identifier -> variable s: integer
											Letter 's'
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
							)
							(ProcedureAndFunctionDeclarationPart)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable s: integer
																	Letter 's'
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
										Semicolon ';'
										(Statement
											(StructuredStatement
												(RepetitiveStatement
													(ForStatement
														For 'for'
														(ControlVariable : integer
															(EntireVariable : integer
																(VariableIdentifier : integer
																	(Identifier -> variable i: integer
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
																			(BoundIdentifier : integer
																				(Identifier -> low bound low: integer
																					Letter 'l'
																					Letter 'o'
																					Letter 'w'
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
																			(BoundIdentifier : integer
																				(Identifier -> high bound high: integer
																					Letter 'h'
																					Letter 'i'
																					Letter 'g'
																					Letter 'h'
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
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable s: integer
																					Letter 's'
																				)
																			)
																		)
																	)
																	Walrus ':='
																	(Expression : integer
																		(SimpleExpression : integer
																			(Term : integer
																				(Factor : integer
																					(VariableAccess : integer
																						(EntireVariable : integer
																							(VariableIdentifier : integer
																								(Identifier -> variable s: integer
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
																			(Term : integer
																				(Factor : integer
																					(VariableAccess : integer
																						(ComponentVariable : integer
																							(IndexedVariable : integer
																								(ArrayVariable : array[integer] of integer
																									(VariableAccess : array[integer] of integer
																										(EntireVariable : array[integer] of integer
																											(VariableIdentifier : array[integer] of integer
																												(Identifier -> parameter a: array[integer] of integer
																													Letter 'a'
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
																													(VariableAccess : integer
																														(EntireVariable : integer
																															(VariableIdentifier : integer
																																(Identifier -> variable i: integer
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
										Semicolon ';'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(FunctionIdentifier
														(Identifier -> function sum: integer
															Letter 's'
															Letter 'u'
															Letter 'm'
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable s: integer
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
									End 'end'
								)
							)
						)
					)
				)
				Semicolon ';'
				(FunctionDeclaration
					(FunctionHeading
						Function 'function'
						(Identifier -> function outer: integer
							Letter 'o'
							Letter 'u'
							Letter 't'
							Letter 'e'
							Letter 'r'
						)
						(FormalParameterList
							OpenParen '('
							(FormalParameterSection
								(ValueParameterSpecification
									(IdentifierList
										(Identifier -> parameter limit: integer
											Letter 'l'
											Letter 'i'
											Letter 'm'
											Letter 'i'
											Letter 't'
										)
									)
									Colon ':'
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
							CloseParen ')'
						)
						Colon ':'
						(ResultType : integer
							(SimpleTypeIdentifier : integer
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
					Semicolon ';'
					(FunctionBlock
						(Block
							(LabelDeclarationPart)
							(ConstantDefinitionPart)
							(TypeDefinitionPart)
							(VariableDeclarationPart)
							(ProcedureAndFunctionDeclarationPart
								(ProcedureDeclaration
									(ProcedureHeading
										Procedure 'procedure'
										(Identifier -> procedure inner
											Letter 'i'
											Letter 'n'
											Letter 'n'
											Letter 'e'
											Letter 'r'
										)
									)
									Semicolon ';'
									(ProcedureBlock
										(Block
											(LabelDeclarationPart)
											(ConstantDefinitionPart)
											(TypeDefinitionPart)
											(VariableDeclarationPart)
											(ProcedureAndFunctionDeclarationPart)
											(StatementPart
												(CompoundStatement
													Begin 'begin'
													(StatementSequence
														(Statement
															(SimpleStatement
																(AssignmentStatement
																	(FunctionIdentifier
																		(Identifier -> function outer: integer
																			Letter 'o'
																			Letter 'u'
																			Letter 't'
																			Letter 'e'
																			Letter 'r'
																		)
																	)
																	Walrus ':='
																	(Expression : integer
																		(SimpleExpression : integer
																			(Term : integer
																				(Factor : integer
																					(VariableAccess : integer
																						(EntireVariable : integer
																							(VariableIdentifier : integer
																								(Identifier -> parameter limit: integer
																									Letter 'l'
																									Letter 'i'
																									Letter 'm'
																									Letter 'i'
																									Letter 't'
																								)
																							)
																						)
																					)
																				)
																				(MultiplyingOperator
																					Asterisk '*'
																				)
																				(Factor : integer
																					(UnsignedConstant : integer
																						(UnsignedNumber : integer
																							(UnsignedInteger : integer
																								(DigitSequence
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
													)
													End 'end'
												)
											)
										)
									)
								)
								Semicolon ';'
							)
							(StatementPart
								(CompoundStatement
									Begin 'begin'
									(StatementSequence
										(Statement
											(SimpleStatement
												(ProcedureStatement
													(ProcedureIdentifier
														(Identifier -> procedure inner
															Letter 'i'
															Letter 'n'
															Letter 'n'
															Letter 'e'
															Letter 'r'
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
				Semicolon ';'
			)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable count: integer
													Letter 'c'
													Letter 'o'
													Letter 'u'
													Letter 'n'
													Letter 't'
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
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(ComponentVariable : integer
											(IndexedVariable : integer
												(ArrayVariable : row
													(VariableAccess : row
														(EntireVariable : row
															(VariableIdentifier : row
																(Identifier -> variable values: row
																	Letter 'v'
																	Letter 'a'
																	Letter 'l'
																	Letter 'u'
																	Letter 'e'
																	Letter 's'
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
									(VariableAccess : integer
										(ComponentVariable : integer
											(IndexedVariable : integer
												(ArrayVariable : row
													(VariableAccess : row
														(EntireVariable : row
															(VariableIdentifier : row
																(Identifier -> variable values: row
																	Letter 'v'
																	Letter 'a'
																	Letter 'l'
																	Letter 'u'
																	Letter 'e'
																	Letter 's'
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
												RightSquareBracket ']'
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
								(AssignmentStatement
									(VariableAccess : integer
										(ComponentVariable : integer
											(IndexedVariable : integer
												(ArrayVariable : row
													(VariableAccess : row
														(EntireVariable : row
															(VariableIdentifier : row
																(Identifier -> variable values: row
																	Letter 'v'
																	Letter 'a'
																	Letter 'l'
																	Letter 'u'
																	Letter 'e'
																	Letter 's'
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
																					Digit '3'
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
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '3'
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
										(Identifier -> procedure swap
											Letter 's'
											Letter 'w'
											Letter 'a'
											Letter 'p'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(VariableAccess : integer
												(ComponentVariable : integer
													(IndexedVariable : integer
														(ArrayVariable : row
															(VariableAccess : row
																(EntireVariable : row
																	(VariableIdentifier : row
																		(Identifier -> variable values: row
																			Letter 'v'
																			Letter 'a'
																			Letter 'l'
																			Letter 'u'
																			Letter 'e'
																			Letter 's'
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
										)
										Comma ','
										(ActualParameter
											(VariableAccess : integer
												(ComponentVariable : integer
													(IndexedVariable : integer
														(ArrayVariable : row
															(VariableAccess : row
																(EntireVariable : row
																	(VariableIdentifier : row
																		(Identifier -> variable values: row
																			Letter 'v'
																			Letter 'a'
																			Letter 'l'
																			Letter 'u'
																			Letter 'e'
																			Letter 's'
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
																							Digit '3'
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
										(Identifier -> procedure apply
											Letter 'a'
											Letter 'p'
											Letter 'p'
											Letter 'l'
											Letter 'y'
										)
									)
									(ActualParameterList
										OpenParen '('
										(ActualParameter
											(FunctionIdentifier
												(Identifier -> function square: integer
													Letter 's'
													Letter 'q'
													Letter 'u'
													Letter 'a'
													Letter 'r'
													Letter 'e'
												)
											)
										)
										Comma ','
										(ActualParameter
											(ProcedureIdentifier
												(Identifier -> procedure later
													Letter 'l'
													Letter 'a'
													Letter 't'
													Letter 'e'
													Letter 'r'
												)
											)
										)
										Comma ','
										(ActualParameter
											(VariableAccess : integer
												(EntireVariable : integer
													(VariableIdentifier : integer
														(Identifier -> variable count: integer
															Letter 'c'
															Letter 'o'
															Letter 'u'
															Letter 'n'
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
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable count: integer
													Letter 'c'
													Letter 'o'
													Letter 'u'
													Letter 'n'
													Letter 't'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function factorial: integer
																Letter 'f'
																Letter 'a'
																Letter 'c'
																Letter 't'
																Letter 'o'
																Letter 'r'
																Letter 'i'
																Letter 'a'
																Letter 'l'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : integer
																	(SimpleExpression : integer
																		(Term : integer
																			(Factor : integer
																				(UnsignedConstant : integer
																					(UnsignedNumber : integer
																						(UnsignedInteger : integer
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
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function sum: integer
																Letter 's'
																Letter 'u'
																Letter 'm'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(VariableAccess : row
																	(EntireVariable : row
																		(VariableIdentifier : row
																			(Identifier -> variable values: row
																				Letter 'v'
																				Letter 'a'
																				Letter 'l'
																				Letter 'u'
																				Letter 'e'
																				Letter 's'
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
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function outer: integer
																Letter 'o'
																Letter 'u'
																Letter 't'
																Letter 'e'
																Letter 'r'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
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
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(EntireVariable : real
											(VariableIdentifier : real
												(Identifier -> variable r: real
													Letter 'r'
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : integer
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable count: integer
																	Letter 'c'
																	Letter 'o'
																	Letter 'u'
																	Letter 'n'
																	Letter 't'
																)
															)
														)
													)
												)
												(MultiplyingOperator
													ForwardSlash '/'
												)
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
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
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable count: integer
																			Letter 'c'
																			Letter 'o'
																			Letter 'u'
																			Letter 'n'
																			Letter 't'
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
											(Expression : real
												(SimpleExpression : real
													(Term : real
														(Factor : real
															(VariableAccess : real
																(EntireVariable : real
																	(VariableIdentifier : real
																		(Identifier -> variable r: real
																			Letter 'r'
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
																			Digit '8'
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
																			Digit '3'
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